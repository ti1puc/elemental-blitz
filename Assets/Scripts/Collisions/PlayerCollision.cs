using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerCollision : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] public int damage = 2;
	[SerializeField] private int invicibilityFrames = 20;
	[SerializeField] private float hitOnTouchEvery = 3f;
	[Header("References")]
	[SerializeField] private ElementManager elementManager;
	[SerializeField] private PlayerController playerController;
	public HealthController healthController;
	[Header("feedback damage")]
	public Material materialDamage;
	public Material materialOriginal;
	public Renderer[] renderers;
	public float durationMax = 0.10f;
	[Header("Debug")]
	[SerializeField, ReadOnly] private bool canBeHit = true;
	[SerializeField, ReadOnly] private float hitOnTouchTimer = 0;
	[SerializeField, ReadOnly] private bool tookDamageOnStay;

	public bool CanBeHit => canBeHit;
	public GameObject shield_;

	#region Unity Messages
	private void Update()
	{
		// buscar scripts no update é pesado pq roda em todo frame, melhor colocar no Awake ou Start e guardar em uma variavel
		//ElementManager elementManager = playerObj.GetComponent<ElementManager>();

		if (durationMax >= 0)
		{
			durationMax -= Time.deltaTime;
		}
		else
		{
			foreach (var renderer in renderers)
				renderer.material = materialOriginal;
		}

		if (tookDamageOnStay)
		{
			hitOnTouchTimer += Time.deltaTime;
			if (hitOnTouchTimer > hitOnTouchEvery)
			{
				hitOnTouchTimer = 0f;
				tookDamageOnStay = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (playerController.IsDashing) return;
		if (!canBeHit) return;

		// touch enemy
		if (other.CompareTag("Enemy") && !tookDamageOnStay)
		{
			Enemy enemy = other.GetComponentInParent<Enemy>();
			if (enemy.IsHitable == false)
				return;

			TakeDamageExternal(1);
			AudioManager.Instance.PlaySFX("snd_Damage");
			tookDamageOnStay = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (playerController.IsDashing) return;
		if (!canBeHit) return;

		BulletBase bullet_ = other.gameObject.GetComponent<BulletBase>();

		//if collide with water element
		#region colision with water
		if (other.CompareTag("enemyWater"))
		{
			playerController.StartSlowDebuff();

			// if current element = lighning

			if (elementManager.CurrentElement == Element.Lightning)
			{
				TakeDamage(bullet_, 0, false);
				AudioManager.Instance.PlaySFX("snd_NoDamage");
			}

			if (elementManager.CurrentElement == Element.Fire)
			{
				TakeDamage(bullet_, 1, true);
				AudioManager.Instance.PlaySFX("snd_CriticalDamage");
			}

            if (elementManager.CurrentElement == Element.Water)
			{
				TakeDamage(bullet_, .5f, true);
				AudioManager.Instance.PlaySFX("snd_Damage");
			}
        }
		#endregion

		#region colision with lighning
		if (other.CompareTag("enemyLightning"))
		{
			playerController.StartStunDebuff();

			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
			{
				TakeDamage(bullet_, .5f, true);
				AudioManager.Instance.PlaySFX("snd_Damage");
			}

			if (elementManager.CurrentElement == Element.Fire)
			{
				TakeDamage(bullet_, 0, false);
				AudioManager.Instance.PlaySFX("snd_NoDamage");
			}

			if (elementManager.CurrentElement == Element.Water)
			{
				TakeDamage(bullet_, 1, true);
				AudioManager.Instance.PlaySFX("snd_CriticalDamage");
			}
        }
		#endregion

		#region colision with fire
		if (other.CompareTag("enemyFire"))
		{
			playerController.StartNoDmgDebuff();

			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
			{
				TakeDamage(bullet_, 1, true);
				AudioManager.Instance.PlaySFX("snd_CriticalDamage");
			}

            if (elementManager.CurrentElement == Element.Fire)
			{
				TakeDamage(bullet_, .5f, true);
				AudioManager.Instance.PlaySFX("snd_Damage");
			}

			// essa logica mudei pro PlayerCollisionField
			// if (elementManager.CurrentElement == Element.Water)
			//{
			//	TakeDamage(bullet_, 0, false);
			//	AudioManager.Instance.PlaySFX("snd_NoDamage");
			//}
        }
		#endregion

		#region colision with power ups
		if (other.CompareTag("pUpHeal"))
		{
			PowerupBase pupb_ = other.gameObject.GetComponent<PowerupBase>();
			healthController.Heal(pupb_.heal_);
			pupb_.DestroyPowerup();
            AudioManager.Instance.PlaySFXPowerUp("snd_PowerUp03");
        }

        if (other.CompareTag("pUpShield"))
        {
            PowerupBase pupb_ = other.gameObject.GetComponent<PowerupBase>();
			
            pupb_.ActiveShield(shield_);
            pupb_.DestroyPowerup();
            AudioManager.Instance.PlaySFXPowerUp("snd_PowerUp03");

		}

		if (other.CompareTag("pUpDash"))
		{
			PowerupBase pupb_ = other.gameObject.GetComponent<PowerupBase>();

			playerController.SetupDash();
			pupb_.DestroyPowerup();
			AudioManager.Instance.PlaySFXPowerUp("snd_PowerUp03");

		}
		#endregion
	}
	#endregion

	// criei essa funcao só pra facilitar
	private void TakeDamage(BulletBase bullet, float damageMultiplier, bool showHitVfx)
	{
		if (HackManager.HasInfiniteHealth == false)
			healthController.TakeDamage(Mathf.CeilToInt(bullet.Damage * damageMultiplier));

		bullet.DestroyBullet();

		if (showHitVfx)
		{
			foreach (var renderer in renderers)
				renderer.material = materialDamage;

			durationMax = 0.10f;
		}

		// se não der dano não ativa frames de invencibilidade
		if (damageMultiplier > 0)
		{
			canBeHit = false;
			StartCoroutine(WaitInvincibilityFrames());
		}
	}

	private IEnumerator WaitInvincibilityFrames()
	{
		for (int i = 0; i < invicibilityFrames; i++)
			yield return null;

		canBeHit = true;
	}

	public void TakeDamageExternal(int damage, bool showHitVfx = true)
	{
		if (!canBeHit) return;

		if (HackManager.HasInfiniteHealth == false)
			healthController.TakeDamage(damage);

		if (showHitVfx)
		{
			foreach (var renderer in renderers)
				renderer.material = materialDamage;

			durationMax = 0.10f;
		}

		canBeHit = false;
		StartCoroutine(WaitInvincibilityFrames());
	}
}

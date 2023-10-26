using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float xPosCorrection;
	[SerializeField] private float zPosCorrection;
	[Header("References")]
	public Enemy enemy;
	public ElementManager elementManager;
	public HealthController healthController;
	[Header("feedback damage")]
	public Material materialDamage;
	public Material materialOriginal;
	public Renderer[] renderers;
	public float durationMax = 0.10f;

	[Header("Debug")]
	[SerializeField, ReadOnly] private float distancePlayer;
	[SerializeField, ReadOnly] private float distanceXZero;
	[SerializeField, ReadOnly] private float differenceXZero;
	[SerializeField, ReadOnly] private float direction;
	#endregion

	#region Properties
	private void Update()
	{
		// mantem a colisao no Y zero
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);

		// calcula o offset do objeto child (localPosition)
		// o offset tem que olhar a distancia do player pra se ajustar
		distancePlayer = Vector3.Distance(PlayerManager.Player.transform.position, transform.position);
		distanceXZero = transform.parent.position.x;
		direction = xPosCorrection == 0 ? 0 : (distanceXZero > 0 ? 1 : -1);
		differenceXZero = Mathf.Abs(distanceXZero) - Mathf.Abs(xPosCorrection);
		if (differenceXZero < 0)
			differenceXZero = 0;

		transform.localPosition = new Vector3(direction * differenceXZero, transform.localPosition.y, zPosCorrection * distancePlayer);


		if (durationMax >= 0)
		{
			durationMax -= Time.deltaTime;
		}
		else
		{
			foreach (var renderer in renderers)
				renderer.material = materialOriginal;
		}
	}
	#endregion

	#region Unity Messages
	private void OnTriggerEnter(Collider other)
	{
		// os inimigos primeiro aparecem na tela, aí rola um feedback mostrando que é possivel atirar nele
		// até esse feedback visual aparecer o inimigo não pode ser hitable

		if (enemy == null) return;
		if (enemy.IsHitable == false) return;

		BulletBase bullet_ = other.gameObject.GetComponent<BulletBase>();

		#region colision with water
		if (other.CompareTag("Water"))
		{
			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
				TakeDamage(bullet_, 0, false);

			if (elementManager.CurrentElement == Element.Fire)
				TakeDamage(bullet_, 1, true);

			if (elementManager.CurrentElement == Element.Water)
				TakeDamage(bullet_, .5f, true);
		}
		#endregion

		#region colision with lighning
		if (other.CompareTag("Lightning"))
		{
			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
				TakeDamage(bullet_, .5f, true);

			if (elementManager.CurrentElement == Element.Fire)
				TakeDamage(bullet_, 0, false);

			if (elementManager.CurrentElement == Element.Water)
				TakeDamage(bullet_, 1, true);
		}
		#endregion

		#region colision with fire
		if (other.CompareTag("Fire"))
		{
			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
				TakeDamage(bullet_, 1, true);

			if (elementManager.CurrentElement == Element.Fire)
				TakeDamage(bullet_, .5f, true);

			if (elementManager.CurrentElement == Element.Water)
				TakeDamage(bullet_, 0, false);
		}
		#endregion
	}
	#endregion

	#region Private Methods
	// criei esse funcao só pra facilitar
	private void TakeDamage(BulletBase bullet, float damageMultiplier, bool showHitVfx)
	{
		healthController.TakeDamage(Mathf.CeilToInt(bullet.Damage * damageMultiplier), enemy.PlayerDestroyEnemy);
		bullet.DestroyBullet();

		if (showHitVfx)
		{
			foreach (var renderer in renderers)
				renderer.material = materialDamage;

			durationMax = 0.10f;
		}
	}
	#endregion
}

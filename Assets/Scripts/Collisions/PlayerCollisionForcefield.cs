using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlayerCollisionForcefield : MonoBehaviour
{
	[SerializeField] private PlayerController playerController;
	[SerializeField] private PlayerCollision playerCollision;
	[SerializeField] private ElementManager elementManager;
	[SerializeField] private ParticleSystem noDamageVfx;

	#region Unity Messages
	private void OnTriggerEnter(Collider other)
	{
		if (!playerCollision.CanBeHit) return;

		BulletBase bullet = other.gameObject.GetComponent<BulletBase>();

		//if collide with water element
		#region colision with water
		if (other.CompareTag("enemyWater"))
		{
			// if current element = lighning
			if (elementManager.CurrentElement == Element.Lightning)
			{
				NoDamage(bullet);
			}
        }
		#endregion

		#region colision with lighning
		if (other.CompareTag("enemyLightning"))
		{
			if (elementManager.CurrentElement == Element.Fire)
			{
				NoDamage(bullet);
			}
		}
		#endregion

		#region colision with fire
		if (other.CompareTag("enemyFire"))
		{
            if (elementManager.CurrentElement == Element.Water)
			{
				NoDamage(bullet);
			}
		}
		#endregion
    }

	private void NoDamage(BulletBase bullet)
	{
		Instantiate(noDamageVfx, bullet.transform.position, noDamageVfx.transform.rotation);
		bullet.DestroyBullet();
		AudioManager.Instance.PlaySFX("snd_NoDamage");
	}
	#endregion
}

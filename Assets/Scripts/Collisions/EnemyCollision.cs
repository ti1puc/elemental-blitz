using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
	#region Fields
	//[Header("Settings")]
	[Header("References")]
	public Enemy enemy;
	public ShootTimed shoot;
	public HealthController healthController;
	//[Header("Debug")]
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void OnTriggerEnter(Collider other)
	{
        BulletBase bullet_ = other.gameObject.GetComponent<BulletBase>();
        // tomar dano e tal
        if (other.CompareTag("Fire"))
        {
			// tal
		}
		if (other.CompareTag("Lightning"))
		{
			// tal
			healthController.TakeDamage(20, enemy.DestroyEnemy);
			GameManager.IncreaseScore(10);
			bullet_.DestroyBullet();
			
		}
		if (other.CompareTag("Water"))
		{
			// tal
		}
	}
	#endregion

	#region Public Methods
	#endregion

	#region Private Methods
	#endregion
}

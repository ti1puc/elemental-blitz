using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolableObject
{
	#region Fields
	//[Header("Settings")]
	[Header("References")]
	[SerializeField] protected Transform enemyVisual;
	[Header("Debug")]
	[SerializeField, ReadOnly] private EnemySpawner parentEnemySpawner;
	[SerializeField, ReadOnly] private PowerupDrop powerupDrop;
	[SerializeField, ReadOnly] private HealthController healthController;
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	private void Awake()
	{
		powerupDrop = GetComponent<PowerupDrop>();
		healthController = GetComponent<HealthController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		// checar se tomou dano

		// diminuir o dano que tomou, o segundo parametro é a funcao q vai chamar se a vida chegar em 0, nesse caso é pra destruir o inimigo
		healthController.TakeDamage(0, DestroyEnemy);
	}
	#endregion

	#region Public Methods
	public void SetParentSpawner(EnemySpawner spawner)
	{
		parentEnemySpawner = spawner;
	}

	[Button]
	public void DestroyEnemy()
	{
		if (powerupDrop)
			powerupDrop.TrySpawnPowerup();

		DestroyPoolable();
	}
	#endregion

	#region Base Methods
	public override void OnDestroyed()
	{
		parentEnemySpawner.DecreaseEnemyCount();
	}
	#endregion

	#region Private Methods
	#endregion
}

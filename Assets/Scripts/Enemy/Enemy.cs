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
	#endregion

	#region Properties
	#endregion

	#region Unity Messages
	#endregion

	#region Public Methods
	public void SetParentSpawner(EnemySpawner spawner)
	{
		parentEnemySpawner = spawner;
	}

	public void DestroyEnemy()
	{
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

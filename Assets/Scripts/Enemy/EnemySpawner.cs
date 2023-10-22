using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	#region Fields
	[Header("Settings")]
	[SerializeField] private float spawnInterval;
	[SerializeField] private int maxEnemies;
	[SerializeField] private Vector2 moveRangeX;
	[SerializeField] private Vector2 moveRangeZ;
	[SerializeField] private GameObject enemyPrefab;
	//[Header("References")]
	[Header("Debug")]
	[SerializeField, ReadOnly] private float timer;
	[SerializeField, ReadOnly] private int currentEnemies;
	[SerializeField, ReadOnly] private bool canSpawn;
	#endregion

	#region Properties
	// can only spawn if current enemies is less than the max
	public bool CanSpawn => canSpawn = currentEnemies < maxEnemies;
	#endregion

	#region Unity Messages
	private void Awake()
	{
		// set timer as interval to spawn enemy when game start
		timer = spawnInterval;
	}

	private void Update()
	{
		if (!CanSpawn) return;

		timer += Time.deltaTime;

		if (timer >= spawnInterval)
		{
			SpawnEnemy();
			timer = 0;
		}
	}
	#endregion

	#region Public Methods
	public void SpawnEnemy()
	{
		if (!CanSpawn) return;

		// find and spawn enemy
		GameObject enemyObj = ObjectPoolManager.SpawnGameObject(enemyPrefab, transform.position, transform.rotation);
		Enemy enemy = enemyObj.GetComponent<Enemy>();
		enemy.InitializeEnemy(moveRangeX, moveRangeZ, this);

		currentEnemies++;
	}

	public void DecreaseEnemyCount()
	{
		currentEnemies--;
	}
	#endregion

	#region Private Methods
	#endregion
}

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[System.Serializable]
	public class SpawnByWaveSettings // criando uma estrutura de dados do C# só pra agrupar valores
	{
		public int WaveIndex;
		public int MaxEnemies;
		public float SpawnInterval;
	}

	#region Fields
	// cada wave tem uma configuracao de spawn diferente
	// isso é só pra evitar varios objetos de spawner iguais que mudam só algumas settings de spawn
	[SerializeField] private List<SpawnByWaveSettings> spawnByWaveSettings = new List<SpawnByWaveSettings>();
	[Header("Overall Spawn Settings")]
	[SerializeField] private Vector2 moveRangeX;
	[SerializeField] private Vector2 moveRangeZ;
	[Header("References")]
	[SerializeField] private GameObject enemyPrefab;
	[Header("Debug")]
	[SerializeField, ReadOnly] private SpawnByWaveSettings currentSettings;
	[SerializeField, ReadOnly] private float timer;
	[SerializeField, ReadOnly] private int currentEnemies;
	[SerializeField, ReadOnly] private bool canSpawn;
	#endregion

	#region Properties
	public SpawnByWaveSettings CurrentSettings
	{
		get
		{
			// colocando como null pro caso de se não achar uma wave compativel
			// é facil de identificar isso
			currentSettings = null;

			foreach (SpawnByWaveSettings setting in spawnByWaveSettings)
            {
				if (setting.WaveIndex == WaveManager.CurrentWave)
					currentSettings = setting;
			}
			return currentSettings;
		}
	}
	public bool HasWaveSetting => CurrentSettings != null;
	public bool CanSpawn
	{
		get
		{
			// can only spawn if current enemies is less than the max
			// e tbm só pode spawnar nas waves selecionadas
			canSpawn = HasWaveSetting && (currentEnemies < CurrentSettings.MaxEnemies);
			return canSpawn;
		}
	}
	#endregion

	#region Unity Messages
	private void Start()
	{
		// set timer as interval to spawn enemy when game start
		if (HasWaveSetting)
			timer = CurrentSettings.SpawnInterval;
	}

	private void Update()
	{
		if (!CanSpawn) return;

		timer += Time.deltaTime;

		if (timer >= CurrentSettings.SpawnInterval)
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

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
	[System.Serializable]
	public class Wave // criando uma estrutura de dados do C# msm pra agrupar valores
	{
		public string Name; // só pra ficar mais facil de identificar
		public float Duration; // em minutos
	}

	[Header("Settings")]
	[SerializeField] private float delayBetweenWaves;
	[SerializeField] private float delayToSpawnBoss = 3;
	[SerializeField] private List<Wave> waves = new List<Wave>();
	[Header("Boss")]
	[SerializeField] private bool bossOnNextScene;
	[SerializeField] private bool startWithBoss;
	[SerializeField] private GameObject bossPrefab;
	[SerializeField] private Transform bossSpawnPoint;
	[SerializeField] private Vector2 bossRangeX = new Vector2(-29, 29);
	[Header("Boss Attack Indicator")]
	[SerializeField] private Animator bossAtackIndicatorLeft;
	[SerializeField] private Animator bossAtackIndicatorCenter;
	[SerializeField] private Animator bossAtackIndicatorRight; 
	[Header("Debug: Wave")]
	[SerializeField, ReadOnly] private int currentWaveIndex;
	[SerializeField, ReadOnly] private Wave currentWave;
	[SerializeField, ReadOnly] private float delayTimer;
	[SerializeField, ReadOnly] private bool isWaitingNextWave;
	[SerializeField, ReadOnly] private float wavePlaytime;
	[SerializeField, ReadOnly] private float wavePlaytimeInMinutes;
	[Header("Debug: Total Playtime")]
	[SerializeField, ReadOnly] private float totalPlaytime;
	[SerializeField, ReadOnly] private float totalPlaytimeInMinutes;
	[SerializeField, ReadOnly] private float levelTotalDuration;
	[Header("Debug: Boss")]
	[SerializeField, ReadOnly] private bool hasStartedBossFight;
	[SerializeField, ReadOnly] private GameObject boss;
	[SerializeField, ReadOnly] private Enemy bossEnemy;
	[SerializeField, ReadOnly] private float bossSpawnTimer;
	[SerializeField, ReadOnly] private float blinkTimer;
	[SerializeField, ReadOnly] private int currentSide;

	public static WaveManager Instance { get; private set; }
	public static int CurrentWave => Instance.currentWaveIndex;
	public static float TotalPlaytimeInMinutes => Instance.totalPlaytimeInMinutes;
	public static float LevelTotalDuration
	{
		get
		{
			Instance.levelTotalDuration = 0;
			foreach (Wave wave in Instance.waves)
				Instance.levelTotalDuration += wave.Duration;
			return Instance.levelTotalDuration;
		}
	}
	public static bool HasStartedBossFight => Instance.hasStartedBossFight;
	public static bool StartWithBoss => Instance.startWithBoss;
	public static GameObject Boss => Instance.boss;
	public static Enemy BossEnemy => Instance.bossEnemy;
	public static bool BossOnNextScene => Instance.bossOnNextScene;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(Instance.gameObject);
			Instance = this;
		}

		if (startWithBoss)
			hasStartedBossFight = true;
		else
		{
			currentWaveIndex = 0;
			currentWave = waves[currentWaveIndex];
		}
	}

	private void Start()
	{
		if (startWithBoss)
		{
			boss = FindObjectOfType<Boss>().gameObject;
			bossEnemy = FindObjectOfType<Enemy>();
		}
	}

	private void Update()
	{
		// cheat pra ir pro boss
		if (Input.GetKeyDown(KeyCode.F5))
		{
			hasStartedBossFight = true;
			currentWaveIndex = waves.Count;
		}

		totalPlaytime += Time.deltaTime;
		totalPlaytimeInMinutes = totalPlaytime / 60f;

		if (HasStartedBossFight && !startWithBoss)
		{
			bossSpawnTimer += Time.deltaTime;
			if (bossSpawnTimer > delayToSpawnBoss && boss == null)
			{
				if (bossOnNextScene)
				{
					GameManager.NextLevel();
					return;
				}

				boss = ObjectPoolManager.Instantiate(bossPrefab, bossSpawnPoint.transform.position, bossSpawnPoint.transform.rotation);
				bossEnemy = boss.GetComponent<Enemy>();
				bossEnemy.InitializeEnemy(bossRangeX, Vector2.zero, null);
			}

			return;
		}

		if (isWaitingNextWave)
		{
			delayTimer += Time.deltaTime;
			if (delayTimer >= delayBetweenWaves)
			{
				delayTimer = 0;
				isWaitingNextWave = false;
			}
		}
		else
		{
			wavePlaytime += Time.deltaTime;
			wavePlaytimeInMinutes = wavePlaytime / 60f;

			// tenta selecionar proxima wave se existir
			TrySelectNextWave();
		}
	}

	public static void BlinkIndicator(int side)
	{
		switch (side)
		{
			case 0:
				Instance.bossAtackIndicatorCenter.SetTrigger("Blink");
				break;
			case 1:
				Instance.bossAtackIndicatorLeft.SetTrigger("Blink");
				break;
			case 2:
				Instance.bossAtackIndicatorRight.SetTrigger("Blink");
				break;
		}
	}

	private void TrySelectNextWave()
	{
		if (wavePlaytimeInMinutes >= currentWave.Duration)
		{
			currentWaveIndex++;
			if (currentWaveIndex > waves.Count - 1)
			{
				// flag sinalizando que vai começar a boss fight
				hasStartedBossFight = true;
				return;
			}

			currentWave = waves[currentWaveIndex];

			// reseta o playtime da wave
			wavePlaytime = 0;

			// coloca flag pra fazer um delay entre waves
			isWaitingNextWave = true;
		}
	}
}

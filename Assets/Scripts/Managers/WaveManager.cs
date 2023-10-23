using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
	[System.Serializable]
	public class Wave // criando uma estrutura de dados do C# msm pra agrupar valores
	{
		public string Name; // s� pra ficar mais facil de identificar
		public float Duration; // em minutos
	}

	[Header("Settings")]
	[SerializeField] private float delayBetweenWaves;
	[SerializeField] private float delayBeforeBossFight;
	[SerializeField] private List<Wave> waves = new List<Wave>();
	[Header("Debug")]
	[SerializeField, ReadOnly] private int currentWaveIndex;
	[SerializeField, ReadOnly] private Wave currentWave;
	[SerializeField, ReadOnly] private float delayTimer;
	[SerializeField, ReadOnly] private bool isWaitingNextWave;
	[SerializeField, ReadOnly] private float wavePlaytime;
	[SerializeField, ReadOnly] private float wavePlaytimeInMinutes;
	[SerializeField, ReadOnly] private float totalPlaytime;
	[SerializeField, ReadOnly] private float totalPlaytimeInMinutes;
	[SerializeField, ReadOnly] private float levelTotalDuration;

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

		currentWaveIndex = 0;
		currentWave = waves[currentWaveIndex];
	}

	private void Update()
	{
		totalPlaytime += Time.deltaTime;
		totalPlaytimeInMinutes = totalPlaytime / 60f;

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

	private void TrySelectNextWave()
	{
		if (wavePlaytimeInMinutes >= currentWave.Duration)
		{
			currentWaveIndex++;
			if (currentWaveIndex > waves.Count - 1)
			{
				// start boss fight here
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

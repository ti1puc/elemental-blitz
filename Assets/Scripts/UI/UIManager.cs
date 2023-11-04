using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[Header("Score")]
	public int scoreToWin = 150;
	public TextMeshProUGUI scoreText;
	public float delayScoreSpeed = 20;
	public Animator scoreAnimator;
	[Header("Warning")]
	[SerializeField] private float bossWarningDuration = 2f;
	[SerializeField] private Animator bossWarning;
	[Header("Menu")]
	[SerializeField] private GameObject pauseScreen;
	[SerializeField] private GameObject winScreen;
	[SerializeField] private GameObject loseScreen;
	[Header("Progression")]
	[SerializeField] private Slider levelProgression;
	[SerializeField] private Animator levelProgressionAnimator;
	[Header("Boss")]
	[SerializeField] private HealthBar bossHealth;
	[SerializeField] private GameObject bossHealthObj;
	[SerializeField] private TMP_Text bossNameText;
	[SerializeField] private HealthBar playerHealth;
	[Header("Element")]
	[SerializeField] private Animator elementChangerAnimator;
	[SerializeField] private Color colorUnlocked = Color.white;
	[SerializeField] private Color colorLocked = Color.grey;
	[SerializeField] private Image lightningSprite;
	[SerializeField] private Image waterSprite;
	[SerializeField] private Image fireSprite;
    [Header("Power Ups")]
    [SerializeField] private HealthBar powerUpTimer;
    [SerializeField] private GameObject Shield;

    [Header("Debug")]
	[SerializeField, ReadOnly] private float scoreDelay = 0;
	[SerializeField, ReadOnly] private bool hasChangedBossMaxHealth;
	[SerializeField, ReadOnly] private bool hasChangedBossName;
    [SerializeField, ReadOnly] private HealthController bossHealthController;
    [SerializeField, ReadOnly] private ShieldPowerUp powerUpTimeController;
    [SerializeField, ReadOnly] private float bossWarningTimer = 0;

	public static UIManager Instance { get; private set; }
	public static HealthBar PlayerLife => Instance.playerHealth;

	#region Unity Messages
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

		// setup inicial
		winScreen.SetActive(false);
		loseScreen.SetActive(false);
		pauseScreen.SetActive(false);
		bossWarning.gameObject.SetActive(false);

		scoreText.text = "0";
		playerHealth.ChangeMaxLife(PlayerManager.PlayerLife.MaxHealth);
	}
	
	private void Update()
	{
		// logica de rotacionar elemento
		lightningSprite.color = PlayerManager.PlayerElement.unlockedElements.Contains(Element.Lightning) ? colorUnlocked : colorLocked;
		waterSprite.color = PlayerManager.PlayerElement.unlockedElements.Contains(Element.Water) ? colorUnlocked : colorLocked;
		fireSprite.color = PlayerManager.PlayerElement.unlockedElements.Contains(Element.Fire) ? colorUnlocked : colorLocked;

		// vida player
		playerHealth.ChangeLife(PlayerManager.PlayerLife.CurrentHealth);

		// vida do boss e progressao
		bossHealthObj.SetActive(WaveManager.HasStartedBossFight && WaveManager.BossEnemy != null && WaveManager.BossEnemy.IsHitable);

		if (WaveManager.HasStartedBossFight)
		{
			levelProgressionAnimator.SetTrigger("ReachBoss");
		}

		if (WaveManager.HasStartedBossFight && WaveManager.BossEnemy != null)
		{
			bossWarningTimer += Time.deltaTime;
			if (bossWarningTimer > bossWarningDuration)
				bossWarning.SetTrigger("BlinkExit");

			bossWarning.gameObject.SetActive(true);
		}

		if (WaveManager.HasStartedBossFight && WaveManager.BossEnemy != null && WaveManager.BossEnemy.IsHitable)
		{
			BossHealth();
			bossWarning.gameObject.SetActive(false);
            
        }
		else
		{
			// progressao do level
			Progression();
		}

		// score
		if (scoreDelay < GameManager.Score)
			Score();

		if (GameManager.IsGameOverWin)
		{
			winScreen.SetActive(true);
			loseScreen.SetActive(false);
		}

		if (GameManager.IsGameOverDefeat)
		{
			winScreen.SetActive(false);
			loseScreen.SetActive(true);
		}

		pauseScreen.SetActive(!GameManager.IsGameOver && GameManager.IsGamePaused);

		PowerUpTimer();
    }
	#endregion

	#region Public Methods
	public static void RotateElement()
	{
		Instance.elementChangerAnimator.SetTrigger("Rotate");
	}

	public void PlayClickSound()
	{
		AudioManager.Instance.PlaySFX("snd_UIClick");
	}

	public void GoToMenu()
	{
		SceneTransition.TransitionToScene(0);
	}

	public void TogglePause()
	{
		GameManager.TogglePause();
	}

	public void NextLevel()
	{
		GameManager.NextLevel();
	}

	public void Retry()
	{
		GameManager.Retry();
	}
	#endregion

	private void Score()
	{
		string formattedScore = scoreDelay.ToString("N0");
		scoreText.text = formattedScore;

		scoreDelay += Time.deltaTime * delayScoreSpeed;
		scoreText.text = formattedScore;

		scoreAnimator.SetBool("playAnimation", true);

		if (scoreDelay >= GameManager.Score)
			scoreAnimator.SetBool("playAnimation", false);
	}

	private void Progression()
	{
		levelProgression.value = WaveManager.TotalPlaytimeInMinutes / WaveManager.LevelTotalDuration;
	}

	private void BossHealth()
	{
		if (bossHealthController == null)
			bossHealthController = WaveManager.Boss.GetComponent<HealthController>();

		if (!hasChangedBossMaxHealth)
		{
			bossHealth.ChangeMaxLife(bossHealthController.MaxHealth);
			hasChangedBossMaxHealth = true;
            AudioManager.Instance.PlayMusic("snd_BossFight1");
        }

		if (!hasChangedBossName)
		{
			bossNameText.text = WaveManager.Boss.GetComponent<Boss>().BossName;
			hasChangedBossName = true;
		}

		bossHealth.ChangeLife(bossHealthController.CurrentHealth);
	}

	private void PowerUpTimer()
	{
		if(powerUpTimeController == null)  powerUpTimeController = Shield.GetComponent<ShieldPowerUp>();

		powerUpTimer.ChangeMaxLife(powerUpTimeController.durationMax);
		powerUpTimer.ChangeLife(powerUpTimeController.Duration);
    }
}

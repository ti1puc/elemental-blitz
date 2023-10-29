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
	[Header("References")]
	[SerializeField] private Button retryButton;
	[SerializeField] private Slider levelProgression;
	[SerializeField] private HealthBar bossHealth;
	[SerializeField] private TMP_Text bossNameText;
	[Header("Debug")]
	[SerializeField, ReadOnly] private float scoreDelay = 0;
	[SerializeField, ReadOnly] private bool hasChangedBossMaxHealth;
	[SerializeField, ReadOnly] private bool hasChangedBossName;
	[SerializeField, ReadOnly] private HealthController bossHealthController;
	[SerializeField, ReadOnly] private float bossWarningTimer = 0;

	#region Unity Messages
	private void Awake()
	{
		//deixando botão desativado
		retryButton.gameObject.SetActive(false);
		bossWarning.gameObject.SetActive(false);

		// colocando o Retry no clique do botão por codigo
		retryButton.onClick.AddListener(Retry);
	}

	private void OnDestroy()
	{
		// removendo a funçào do clique quando o objeto for destruido
		// porque pode acontecer de ficar registrado a funçao e gerar erros
		retryButton.onClick.RemoveListener(Retry);
	}
	
	private void Update()
	{
		levelProgression.transform.parent.gameObject.SetActive(WaveManager.HasStartedBossFight == false);
		bossHealth.transform.parent.gameObject.SetActive(WaveManager.HasStartedBossFight && WaveManager.BossEnemy != null && WaveManager.BossEnemy.IsHitable);

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

		if (GameManager.Score >= scoreToWin)
		{
			scoreText.text = "You win!";
			GameManager.Win();
		}

		if (GameManager.IsGameOverDefeat)
			scoreText.text = "You lose!";

		if (GameManager.IsGameOver)
			retryButton.gameObject.SetActive(true);
	}
	#endregion

	public void Retry()
	{
		GameManager.Retry();
	}

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
		}

		if (!hasChangedBossName)
		{
			bossNameText.text = WaveManager.Boss.GetComponent<Boss>().BossName;
			hasChangedBossName = true;
		}

		bossHealth.ChangeLife(bossHealthController.CurrentHealth);
	}
}

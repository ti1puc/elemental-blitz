using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Fields
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] protected bool dontDestroyOnLoad = true;
    [Header("Random")]
    [SerializeField] private int seed;
    [Header("Score")]
    public float score = 0;
    [SerializeField] private float scoreToWin = 150;
	public TextMeshPro scoreUi;
    public float delayScoreSpeed = 20;
    public Animator scoreAnimator;
    [Header("References")]
    public GameObject player_;
    public Button retryButton;

    private float scoreDelay = 0;
    private HealthController playerLife;
    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    private void Awake()
	{
        // inicializa a funçao que faz esse manager persistir em qualquer cena e ser accessado sem precisar de referencia
        InitializeSingleton();
		Random.InitState(seed);

        // colocando o Retry no clique do botão por codigo
        retryButton.onClick.AddListener(Retry);
	}

	private void OnDestroy()
	{
        // removendo a funçào do clique quando o objeto for destruido
        // porque pode acontecer de ficar registrado a funçao e gerar erros
        retryButton.onClick.RemoveListener(Retry);
	}

	private void Start()
	{
        // pegando a vida e colocando numa variavel cacheada
        playerLife = player_.GetComponent<HealthController>();

		//deixando botão desativado
		retryButton.gameObject.SetActive(false);
	}

	private void Update()
    {
        // score
        if (scoreDelay < score) Score();

        if (score >= scoreToWin) Win();

		// evita usar GetComponent no Update por que isso vai rodar em todo frame, aí fica muito pesado
		// melhor buscar esse component 1 vez só no Awake ou Start
		//HealthController life = player_.GetComponent<HealthController>();

		if (playerLife.CurrentHealth <= 0) Defeat();
    }
    #endregion

    #region Public Methods
    public static void IncreaseScore(int scoreAdd)
    {
        Instance.score += scoreAdd;
    }

    [Button]
    public void Retry()
    {
        SceneManager.LoadScene("Prototype");
    }
    #endregion

    #region Private Methods
    private void InitializeSingleton()
    {
		if (Instance == null)
		{
			Instance = this;

			if (dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(Instance.gameObject);
			Instance = this;

			if (dontDestroyOnLoad)
				DontDestroyOnLoad(gameObject);
		}
	}

    private void Score()
    {
        string formattedScore = scoreDelay.ToString("N0");
        scoreUi.text = formattedScore;

        scoreDelay += Time.deltaTime * delayScoreSpeed;
        scoreUi.text = formattedScore;

        scoreAnimator.SetBool("playAnimation", true);

        if(scoreDelay >= score) scoreAnimator.SetBool("playAnimation", false);
    }

    private void Win()
    {
        scoreUi.text = "You win!";
        retryButton.gameObject.SetActive(true);
        player_.SetActive(false);

    }
    private void Defeat()
    {
        scoreUi.text = "You lose!";
        retryButton.gameObject.SetActive(true);
        player_.SetActive(false);
    }

    [Button]
    private void IncreaseScoreBy10()
	{
		score += 10;
	}
	#endregion
}

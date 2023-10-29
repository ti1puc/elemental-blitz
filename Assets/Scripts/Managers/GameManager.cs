using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Fields
    public static GameManager Instance { get; private set; }

    [Header("Singleton")]
    [SerializeField] protected bool dontDestroyOnLoad = true;
    [Header("Random")]
    [SerializeField] private int seed;
    // SCORE -> passei as coisas de score pro UIManager
    [Header("Debug")]
	[SerializeField, ReadOnly] private float score = 0;
    [SerializeField, ReadOnly] private bool isGameOverWin;
    [SerializeField, ReadOnly] private bool isGameOverDefeat;
    [SerializeField, ReadOnly] private bool isGamePaused;
    [SerializeField, ReadOnly] private HealthController bossHealthController;
	#endregion

	#region Properties
	public static float Score => Instance.score;
	public static bool IsGameOver => Instance.isGameOverWin || Instance.isGameOverDefeat;
    public static bool IsGameOverWin => Instance.isGameOverWin;
	public static bool IsGameOverDefeat => Instance.isGameOverDefeat;
	public static bool IsGamePaused => Instance.isGamePaused;
	#endregion

	#region Unity Messages

	private void Awake()
    {
        // inicializa a funçao que faz esse manager persistir em qualquer cena e ser accessado sem precisar de referencia
        InitializeSingleton();
        Random.InitState(seed);

        // despausa o game no inicio da cena
        Time.timeScale = 1f;
    }

	private void Update()
    {
		// evita usar GetComponent no Update por que isso vai rodar em todo frame, aí fica muito pesado
		// melhor buscar esse component 1 vez só no Awake ou Start
		//HealthController life = player_.GetComponent<HealthController>();

		if (bossHealthController == null)
			bossHealthController = WaveManager.Boss.GetComponent<HealthController>();

		if (bossHealthController.CurrentHealth <= 0)
			Win();

		if (PlayerManager.PlayerLife.CurrentHealth <= 0)
            Defeat();

		if (Input.GetKeyDown(KeyCode.Escape))
			TogglePause();
    }
    #endregion

    #region Public Methods
    public static void IncreaseScore(int scoreAdd)
    {
        Instance.score += scoreAdd;
    }

    public static void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void TogglePause()
	{
		Instance.isGamePaused = !Instance.isGamePaused;
		Time.timeScale = Instance.isGamePaused ? 0f : 1f;
	}

	public static void NextLevel()
	{
		// logica pra ir pro prox level
	}

	public static void Win()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = true;
		Instance.isGameOverDefeat = false;
	}
	public static void Defeat()
	{
		// pause game
		Time.timeScale = 0f;

		Instance.isGameOverWin = false;
		Instance.isGameOverDefeat = true;
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
	#endregion
}

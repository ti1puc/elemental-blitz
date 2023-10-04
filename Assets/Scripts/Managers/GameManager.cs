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
    public TextMeshPro scoreUi;
    public float delayScoreSpeed = 20;
    public Animator scoreAnimator;
    private float scoreDelay = 0;
    [Header("References")]
    public GameObject player_;
    public Button retryButton;
    

    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    public void Start()
    {
        //deixando botão desativado
        retryButton.gameObject.SetActive(false);
    }

    private void Awake()
	{
		Random.InitState(seed);

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

	private void Update()
    {
        // score
        if (scoreDelay < score) Score();

        if (score >= 150) Win();

        HealthController life = player_.GetComponent<HealthController>();
        if (life.CurrentHealth <= 0) Defeat();
    }
    #endregion

    #region Public Methods
    public static void IncreaseScore(int scoreAdd)
    {
        Instance.score += scoreAdd;
    }
    public void Retry()
    {
        SceneManager.LoadScene("Prototype");
    }
    #endregion

    #region Private Methods
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
    private void TestIncreaseScore()
	{
		score += 10;
	}
	#endregion
}

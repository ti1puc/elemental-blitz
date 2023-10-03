using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    #endregion

    #region Properties
    #endregion

    #region Unity Messages

    public void Start()
    {
        
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
        if (scoreDelay < score)
            Score();

    }
    #endregion

    #region Public Methods
    public static void IncreaseScore(int scoreAdd)
    {
        Instance.score += scoreAdd;
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

    [Button]
    private void TestIncreaseScore()
	{
		score += 10;
	}
	#endregion
}

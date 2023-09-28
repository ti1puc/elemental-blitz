using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : SingletonManager
{
    #region Fields
    public float score = 0;
    public TextMeshPro scoreUi;
    public float delayScoreSpeed = 20;
    public Animator scoreAnimator;

    private float scoreDelay = 0;
    #endregion

    #region Properties
	#endregion

	#region Unity Messages
	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
    {
        // score
        if (scoreDelay < score)
            Score();

    }
    #endregion

    #region Public Methods
	#endregion

	#region Private Methods
	private void Score()
    {
        string formattedScore = scoreDelay.ToString("N0");
        scoreUi.text = formattedScore;

        scoreDelay += Time.deltaTime * delayScoreSpeed;
        scoreUi.text = formattedScore;

        scoreAnimator.enabled = true;

        if(scoreDelay >= score) scoreAnimator.enabled = false;
    }

    [Button]
    private void TestIncreaseScore()
	{
		score += 10;
	}
	#endregion
}

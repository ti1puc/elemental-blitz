using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
	[SerializeField] private Animator transitionAnimator;
	[SerializeField] private float transitionTime = 1f;

	public static SceneTransition Instance { get; private set; }

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

		Time.timeScale = 1f;
	}

	#region Next Level
	public static void TransitionToNextLevel()
	{
		Instance.Transition();
	}

	public void Transition()
	{
		transitionAnimator.SetTrigger("Fade");
		StartCoroutine(TransitionCoroutine());
	}

	private IEnumerator TransitionCoroutine()
	{
		yield return new WaitForSecondsRealtime(Instance.transitionTime);

		Time.timeScale = 1f;
		if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
			SceneManager.LoadScene(0);
		else
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	#endregion

	#region Select Level
	public static void TransitionToScene(int sceneIndex)
	{
		Instance.TransitionTo(sceneIndex);
	}

	public void TransitionTo(int sceneIndex)
	{
		transitionAnimator.SetTrigger("Fade");
		StartCoroutine(TransitionToCoroutine(sceneIndex));
	}

	private IEnumerator TransitionToCoroutine(int sceneIndex)
	{
		yield return new WaitForSecondsRealtime (Instance.transitionTime);

		Time.timeScale = 1f;
		SceneManager.LoadScene(sceneIndex);
	}
	#endregion
}

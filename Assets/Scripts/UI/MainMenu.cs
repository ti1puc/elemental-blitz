using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject optionPanel;
	[SerializeField] private float waitSecondsToStart = 1;
	[SerializeField] private Animator playerMenuAnimator;

	private void Awake()
	{
		optionPanel.SetActive(false);
	}

	public void StartGame()
	{
		playerMenuAnimator.SetTrigger("Go");
		StartCoroutine(StartGameDelay());
	}

	public void ShowOptionPanel()
	{
		optionPanel.SetActive(true);
	}

	public void HideOptionPanel()
	{
		optionPanel.SetActive(false);
	}

	private IEnumerator StartGameDelay()
	{
		yield return new WaitForSeconds(waitSecondsToStart);
		GameManager.StartGame();
	}
}

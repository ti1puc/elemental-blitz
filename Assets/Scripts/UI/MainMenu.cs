using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject optionPanel;
	[SerializeField] private float waitSecondsToStart = 1;
	[SerializeField] private Animator playerMenuAnimator;
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;

	private const string sfxVolumeKey = "SFXVolumeValue";
	private const string musicVolumeKey = "MusicVolumeValue";

	private void Awake()
	{
		optionPanel.SetActive(false);

		sfxSlider.value = PlayerPrefs.GetInt(sfxVolumeKey, 50) / (float)100;
		musicSlider.value = PlayerPrefs.GetInt(musicVolumeKey, 50) / (float)100;
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

	public void SaveSfxVolume(Single value)
	{
		int actualValue = Mathf.RoundToInt(value * 100);
		PlayerPrefs.SetInt(sfxVolumeKey, actualValue);
	}

	public void SaveMusicVolume(Single value)
	{
		int actualValue = Mathf.RoundToInt(value * 100);
		PlayerPrefs.SetInt(musicVolumeKey, actualValue);
	}

	private IEnumerator StartGameDelay()
	{
		yield return new WaitForSeconds(waitSecondsToStart);
		GameManager.StartGame();
	}
}

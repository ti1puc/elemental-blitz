using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[Header("Start Animation")]
	[SerializeField] private Animator playerMenuAnimator;
	[SerializeField] private float waitSecondsToStart = 1;
	[Header("Panels")]
	[SerializeField] private GameObject optionPanel;
	[SerializeField] private GameObject helpPanel;
	[SerializeField] private GameObject creditsPanel;
	[Header("Options")]
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;

	private const string sfxVolumeKey = "SFXVolumeValue";
	private const string musicVolumeKey = "MusicVolumeValue";

	private void Awake()
	{
		HideAllPanels();

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

	public void ShowHelpPanel()
	{
		helpPanel.SetActive(true);
	}

	public void ShowCreditsPanel()
	{
		creditsPanel.SetActive(true);
	}

	public void HideAllPanels()
	{
		optionPanel.SetActive(false);
		helpPanel.SetActive(false);
		creditsPanel.SetActive(false);
	}

	public void PlayClickSound()
	{
		AudioManager.Instance.PlaySFXUI("snd_UIClick");
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

using NaughtyAttributes;
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
	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject optionPanel;
	[SerializeField] private GameObject creditsPanel;
	[Header("Tutorial")]
	[SerializeField] private GameObject helpPanel;
	[SerializeField] private Button arrowLeft;
	[SerializeField] private Button arrowRight;
	[SerializeField] private List<GameObject> helpPages;
	[SerializeField] private Animator elementChangerAnimator;
	[SerializeField] private Animator elementChangerAnimator2;
	[Header("Options")]
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;
	[Header("Debug")]
	[SerializeField, ReadOnly] private int helpPage;
	[SerializeField, ReadOnly] private bool wasOnHelpPage;

	private const string sfxVolumeKey = "SFXVolumeValue";
	private const string musicVolumeKey = "MusicVolumeValue";

	private void Awake()
	{
		HideAllPanels();

		sfxSlider.value = PlayerPrefs.GetInt(sfxVolumeKey, 50) / (float)100;
		musicSlider.value = PlayerPrefs.GetInt(musicVolumeKey, 50) / (float)100;
	}

	private void Update()
	{
		arrowLeft.gameObject.SetActive(helpPage > 0);
		arrowRight.gameObject.SetActive(helpPage < helpPages.Count-1);

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			elementChangerAnimator.SetTrigger("Rotate");
			elementChangerAnimator2.SetTrigger("Rotate");
		}
	}

	public void StartGame()
	{
		AudioManager.Instance.PlaySFXUI("snd_Start");
		playerMenuAnimator.SetTrigger("Go");
		StartCoroutine(StartGameDelay());
	}

	public void ShowOptionPanel()
	{
		HideAllPanels();
		mainPanel.SetActive(false);
		optionPanel.SetActive(true);
	}

	public void ShowHelpPanel()
	{
		HideAllPanels();
		mainPanel.SetActive(false);
		helpPanel.SetActive(true);

		helpPage = 0;
		UpdateHelpPanel();

		wasOnHelpPage = true;
		playerMenuAnimator.SetTrigger("Out");

        AudioManager.Instance.PlayMusic("snd_Tutorial -  Prebattle");
    }

	public void ShowCreditsPanel()
	{
		HideAllPanels();
		mainPanel.SetActive(false);
		creditsPanel.SetActive(true);

        AudioManager.Instance.PlayMusic("snd_ Thanks");
    }

	public void HideAllPanels()
	{
		if (wasOnHelpPage)
		{
			playerMenuAnimator.SetTrigger("In");
			wasOnHelpPage = false;
		}

		optionPanel.SetActive(false);
		helpPanel.SetActive(false);
		creditsPanel.SetActive(false);
		mainPanel.SetActive(true);

		AudioManager.Instance.PlayMusic("snd_menu");
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

	public void PassHelpPanelToRight()
	{
		helpPage++;
		UpdateHelpPanel();
	}

	public void PassHelpPanelToLeft()
	{
		helpPage--;
		UpdateHelpPanel();
	}

	private void UpdateHelpPanel()
	{
		for (int i = 0; i < helpPages.Count; i++)
        {
			helpPages[i].SetActive(i == helpPage);
        }
    }

	private IEnumerator StartGameDelay()
	{
		yield return new WaitForSeconds(waitSecondsToStart);
		GameManager.StartGame();
	}
}

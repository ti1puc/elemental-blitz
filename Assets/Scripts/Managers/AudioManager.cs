using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSourceEnemy;
	public AudioSource sfxAudioSourcePowerUp;
    public AudioSource sfxAudioSourceUI;
    public AudioSource sfxAudioSourceBoss;

	private const string sfxVolumeKey = "SFXVolumeValue";
    private const string musicVolumeKey = "MusicVolumeValue";

    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
    }

    int scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;

        if (scene == 0) PlayMusic("snd_menu");
		if (scene == 1) PlayMusic("snd_Cutscene");
		if (scene == 2) PlayMusic("snd_Level_1");
        if (scene == 3) PlayMusic("snd_Cutscene");
		if (scene == 4) PlayMusic("snd_Level_2");
		if (scene == 5) PlayMusic("snd_Cutscene");
		if (scene == 6) PlayMusic("snd_Level_3");
	}

    public void Update()
    {
        float sfxVolume = PlayerPrefs.GetInt(sfxVolumeKey, 50) / 100.0f;
        float musicVolume = PlayerPrefs.GetInt(musicVolumeKey, 50) / 100.0f;

        sfxAudioSource.volume = sfxVolume;
        musicAudioSource.volume = musicVolume;

        sfxAudioSourceEnemy.volume = sfxVolume;
		sfxAudioSourceBoss.volume = sfxVolume;
        sfxAudioSourcePowerUp.volume = sfxVolume;
		sfxAudioSourceUI.volume = sfxVolume;
	}

    public void PlayMusic(string audioName)
    {
        AudioClip musicClip = Resources.Load<AudioClip>("Sounds/Musics/" + audioName);

        if (musicClip != null)
        {
            if (musicAudioSource.isPlaying && musicAudioSource.clip == musicClip)
            {
                // A m�sica espec�fica j� est� tocando
                Debug.Log("A m�sica '" + audioName + "' j� est� tocando.");
            }
            else
            {
                // A m�sica n�o est� tocando ou � diferente, reproduza-a
                musicAudioSource.clip = musicClip;
                musicAudioSource.Play();
                musicAudioSource.loop = true;
            }
        }
        else
        {
            Debug.LogError("�udio '" + audioName + "' n�o encontrado.");
        }
    }

    public void PlaySFX(string audioName)
	{
		PlaySound(audioName, sfxAudioSource);
	}

	public void PlaySFXEnemy(string audioName)
    {
		PlaySound(audioName, sfxAudioSourceEnemy);
	}

	public void PlaySFXBoss(string audioName)
	{
		PlaySound(audioName, sfxAudioSourceBoss);
	}

	public void StopSoundBoss()
	{
		sfxAudioSourceBoss.Stop();
	}

	public void PlaySFXPowerUp(string audioName)
    {
		PlaySound(audioName, sfxAudioSourcePowerUp);
	}

	public void PlaySFXUI(string audioName)
	{
        PlaySound(audioName, sfxAudioSourceUI);
	}

    private void PlaySound(string audioName, AudioSource audioSource)
	{
		AudioClip sfxClip = Resources.Load<AudioClip>("Sounds/Sfx/" + audioName);
		if (sfxClip != null)
		{
			audioSource.clip = sfxClip;
			audioSource.Play();
		}
		else
		{
			Debug.LogError("�udio '" + audioName + "' n�o encontrado.");
		}
	}
}

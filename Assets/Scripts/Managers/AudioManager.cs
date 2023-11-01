using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSourceEnemy;
    public AudioSource sfxAudioSourcePowerUp;

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
        if (scene == 1) PlayMusic("snd_Level_1");
        if (scene == 2) PlayMusic("snd_Level_2");
        if (scene == 3) PlayMusic("snd_Level_3");

        
    }

    public void Update()
    {
       
            float sfxVolume = PlayerPrefs.GetInt(sfxVolumeKey, 50) / 100.0f;
            float musicVolume = PlayerPrefs.GetInt(musicVolumeKey, 50) / 100.0f;

            sfxAudioSource.volume = sfxVolume;
            musicAudioSource.volume = musicVolume;

            sfxAudioSourceEnemy.volume = sfxVolume;
            sfxAudioSourcePowerUp.volume = sfxVolume;
    }

    public void PlaySFX(string audioName)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>("Sounds/Sfx/" + audioName);
        if (sfxClip != null)
        {
            sfxAudioSource.clip = sfxClip;
            sfxAudioSource.Play();
        }
        else
        {
            Debug.LogError("Áudio '" + audioName + "' não encontrado.");
        }
    }

    public void PlayMusic(string audioName)
    {
        AudioClip MusicClip = Resources.Load<AudioClip>("Sounds/Musics/" + audioName);
        if (MusicClip != null)
        {
            musicAudioSource.clip = MusicClip;
            musicAudioSource.Play();
            musicAudioSource.loop = true;
        }
        else
        {
            Debug.LogError("Áudio '" + audioName + "' não encontrado.");
        }
    }

    public void PlaySFXEnemy(string audioName)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>("Sounds/Sfx/" + audioName);
        if (sfxClip != null)
        {
            sfxAudioSourceEnemy.clip = sfxClip;
            sfxAudioSourceEnemy.Play();
        }
        else
        {
            Debug.LogError("Áudio '" + audioName + "' não encontrado.");
        }
    }

    public void PlaySFXPowerUp(string audioName)
    {
        AudioClip sfxClip = Resources.Load<AudioClip>("Sounds/Sfx/" + audioName);
        if (sfxClip != null)
        {
            sfxAudioSourcePowerUp.clip = sfxClip;
            sfxAudioSourcePowerUp.Play();
        }
        else
        {
            Debug.LogError("Áudio '" + audioName + "' não encontrado.");
        }
    }

}

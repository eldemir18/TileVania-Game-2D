using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMusic : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        int numberOfAudioSources = FindObjectsOfType<BackGroundMusic>().Length;

        if(numberOfAudioSources > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SetMusicVolume();
    }
    
    public void SetMusicVolume()
    {
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }
}

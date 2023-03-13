using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectSlider;
    
    void Start()
    {
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 0.5f);      
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void SetEffectSlider()
    {
        PlayerPrefs.SetFloat("EffectVolume", effectSlider.value); 
    }

    public void SetMusicSlider()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value); 
        GameObject.FindGameObjectWithTag("Music").GetComponent<BackGroundMusic>().SetMusicVolume();
    }
}

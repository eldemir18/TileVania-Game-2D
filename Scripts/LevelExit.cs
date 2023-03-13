using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] AudioClip finishSFX;
    [SerializeField] float levelLoadDelay = 1f;
    GameSession gameSession;

    private bool isExitTriggered = false;
    private float volume;

    void Start()
    {
        volume =  PlayerPrefs.GetFloat("EffectVolume",0.5f);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (!isExitTriggered) 
        {
            isExitTriggered = true;
            FindObjectOfType<PlayerMovement>().DisableMovement();
            gameSession = FindObjectOfType<GameSession>();
            AudioSource.PlayClipAtPoint(finishSFX, Camera.main.transform.position, volume);
            // Popup will appear
            StartCoroutine(LoadMainMenu());
        }
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        
        gameSession.UpdateLevelStars();
        gameSession.ResetGameSession();
        
        SceneManager.LoadScene("Main Menu");

        isExitTriggered = false;
    }
}

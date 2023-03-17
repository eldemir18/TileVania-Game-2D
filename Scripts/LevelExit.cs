using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelExit : MonoBehaviour
{
    
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image star0;
    [SerializeField] Image star1;
    [SerializeField] Image star2;
    [SerializeField] Sprite filledStarSprite;

    [Space]
    
    [SerializeField] AudioClip starPopSFX;

    [Space]

    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] float starLoadDelay = 0.5f;
    
    Camera mainCamera;
    GameSession gameSession;
    ScenePersist scenePersist;

    private bool isExitTriggered = false;
    private float volume;

    void Awake()
    {
        mainCamera = Camera.main;
        gameSession = FindObjectOfType<GameSession>();
    }

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
              
            gameSession.UpdateLevelStars();
            // Popup will appear
            StartCoroutine(OpenGameOverCanvas());
        }
    }

    IEnumerator OpenGameOverCanvas()
    {
        yield return new WaitForSeconds(levelLoadDelay);

        gameOverCanvas.SetActive(true);
        scoreText.text = gameSession.CurrentScore.ToString();
        
        yield return new WaitForSeconds(gameOverCanvas.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length);


        // star scores
        if(gameSession.CurrentScore > gameSession.StarScores[0])
        {
            star0.sprite = filledStarSprite;
            AudioSource.PlayClipAtPoint(starPopSFX, mainCamera.transform.position, volume);
            yield return new WaitForSeconds(starLoadDelay);
        }

        if(gameSession.CurrentScore > gameSession.StarScores[1])
        {
            star1.sprite = filledStarSprite;
            AudioSource.PlayClipAtPoint(starPopSFX, mainCamera.transform.position, volume);
            yield return new WaitForSeconds(starLoadDelay);
        }

        if(gameSession.CurrentScore > gameSession.StarScores[2])
        {
            star2.sprite = filledStarSprite;
            AudioSource.PlayClipAtPoint(starPopSFX, mainCamera.transform.position, volume);
        }
    }

    public void LoadMainMenu()
    {
        ResetGameInfo();

        SceneManager.LoadScene("Main Menu");
    }
    
    public void ReplayLevel()
    {
        ResetGameInfo();

        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Load the current scene
        SceneManager.LoadScene(currentSceneName);
    }

    private void ResetGameInfo()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        
        gameSession.ResetGameSession();
    }


}

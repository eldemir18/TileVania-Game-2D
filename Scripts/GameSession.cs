using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int currentScore = 0;
    [SerializeField] int[] starScores = new int[3];
    [SerializeField] int level;

    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] Image[] hearts;
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int numberOfSessions = FindObjectsOfType<GameSession>().Length;

        if(numberOfSessions > 1)
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
        SetHearts();
        scoreText.text = currentScore.ToString();
    }

    void SetHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < playerLives)
            {
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].color = Color.white;
            }
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = currentScore.ToString();
    }

    public void ProcessPlayerDeath()
    {
        UpdateHearts(false);

        StartCoroutine(LoadScene(playerLives > 0));
    }

    IEnumerator LoadScene(bool isAlive)
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        if(isAlive)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene("Main Menu");   
            Destroy(gameObject);
        }
    }

    public void UpdateHearts(bool direction)
    {
        if(direction)
        {
            hearts[playerLives].color = Color.red;
            playerLives++;
        }
        else if(!direction)
        {
            hearts[playerLives-1].color = Color.white;
            playerLives--;   
        }
        //Debug.Log("Player Lives: " + playerLives);
    }

    public void UpdateLevelStars()
    {
        //Debug.Log("Player Lives: " + playerLives);
        currentScore += playerLives * 500;
        //Debug.Log("Current Score: " + currentScore);
        int currentStar = PlayerPrefs.GetInt("LevelStar" + level.ToString(), 0);

        for(int i = 2; i >= 0; i--)
        {
            if(currentScore >= starScores[i] && currentStar < i+1)
            {
                PlayerPrefs.SetInt("LevelStar" + level.ToString(), i+1);
                PlayerPrefs.SetInt("UnlockedLevel", level+1);
                break;
            }
        }
    }

    public bool IsHeartsFull()
    {
        return playerLives == hearts.Length;
    }

    public void ResetGameSession()
    {
        Destroy(gameObject);
    }
}

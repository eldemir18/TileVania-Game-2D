using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    [SerializeField] Sprite fillStarSprite;

    private int level;

    public void SetLevelButton(bool isUnlocked)
    {
        gameObject.GetComponent<Button>().interactable = isUnlocked;

        if(isUnlocked)
        {
            SetStars();
        }
        else
        {
            gameObject.GetComponent<Image>().color =  new Color(0.5f, 0.5f, 0.5f);
            gameObject.transform.Find("Lock").gameObject.SetActive(true);
        }
    }

    public void SetLevel(int _level)
    {   
        level = _level;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + level.ToString();
    }

    public void SetStars()
    {
        //Debug.Log("Setting Stars!");
        int starCount = PlayerPrefs.GetInt("LevelStar" + level.ToString(), 0);
        //Debug.Log("StarCount: " + starCount);
        for (int i = 0; i < starCount; i++)
        {
            stars[i].GetComponent<Image>().sprite = fillStarSprite;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }
}

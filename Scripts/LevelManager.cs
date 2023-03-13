using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levelPrefabs;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject prevButton;

    LevelButton levelButton;
    private int totalLevels;
    private int unlockedLevel; 
    private int totalPages;
    private int page;
    private int pageItem = 3;

    void Start()
    {
        totalLevels = levelPrefabs.Count;
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        Debug.Log("Unlocked Level: " + unlockedLevel);
        totalPages = (int) Mathf.Ceil((float) totalLevels / pageItem);
        page = (unlockedLevel - 1) / pageItem + 1;

        SetLevels();
    }

    public void SetLevels()
    {
        int startLevel = (page - 1) * pageItem + 1;
        int endLevel = Mathf.Min(page * pageItem, totalLevels);

        for (int i = 0; i < totalLevels; i++)
        {
            int level = i + 1;

            levelButton = levelPrefabs[i].GetComponent<LevelButton>();
            levelButton.SetLevel(level);

            if(startLevel <= level && endLevel >= level)
            {
                levelPrefabs[i].SetActive(true);
                levelButton.SetLevelButton(i < unlockedLevel);
            }
            else
            {
                levelPrefabs[i].SetActive(false);
            }
        }

        CheckButton();
    }

    public void NextPage()
    {
        page += 1;
        SetLevels();
    }

    public void PrevPage()
    {
        page -= 1;
        SetLevels();
    }

    private void CheckButton()
    {
        nextButton.SetActive(page < totalPages);
        prevButton.SetActive(page > 1);
    }
}

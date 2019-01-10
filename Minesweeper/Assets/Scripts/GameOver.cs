using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public GameObject GameOverPanel;

    public Text WinText;
    public Text LoseText;
    public Text CurrentTimeText;
    public Text BestTimeText;
    public Text BestTimeDateText;
    public Text GamesPlayedText;
    public Text GamesWonText;
    public Text PercentageText;

    public void Show(bool victory)
    {
        if (victory)
        {
            WinText.enabled = true;
            LoseText.enabled = false;
        }
        else
        {
            WinText.enabled = false;
            LoseText.enabled = true;
        }

        int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed");
        int gamesWon = PlayerPrefs.GetInt("GamesWon", 0);
        int percentWon = Mathf.RoundToInt((float)gamesWon/(float)gamesPlayed * 100);

        CurrentTimeText.text = "Current Time: " + Mathf.RoundToInt(gm.currentTime) + " seconds";
        BestTimeText.text = "Best Time: " + PlayerPrefs.GetInt("BestTime", 0) + " seconds";
        BestTimeDateText.text = "Date: " + PlayerPrefs.GetString("BestTimeDate", "Never");
        GamesPlayedText.text = "Games Played: " + gamesPlayed;
        GamesWonText.text = "Games Won: " + gamesWon;
        PercentageText.text = "Percentage: " + percentWon + "%";

        GameOverPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

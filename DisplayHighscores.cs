using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayHighscores : MonoBehaviour
{

    public Text leaderboardContent;
    public ScrollRect leaderboardScroller;
    Highscores highscoreManager;

    // Use this for initialization
    void Start()
    {
        highscoreManager = GetComponent<Highscores>();
        StartCoroutine("RefreshHighscores");
    }

    public void refreshHighScoresFromMenu()
    {
        highscoreManager.loadingIndicator.gameObject.SetActive(true);
        StartCoroutine("RefreshHighscores");
    }

    public void OnHighscoresDownloaded(Highscore[] highscoreList)
    {
        int offset = 0;
        leaderboardContent.text = "";
        for (int i = 0; i < highscoreList.Length; i++)
        {
            if (highscoreList[i].check == (highscoreManager.stringToInt(highscoreList[i].playerName) + highscoreManager.encryption(highscoreList[i].skill, highscoreList[i].playerName)).ToString())
            {
                leaderboardContent.text += (i + 1 - offset) + ". ";
                leaderboardContent.text += highscoreList[i].skill / 1000 + " - " + highscoreList[i].ship; // + " - " + highscoreList[i].playerName;
                leaderboardContent.text += Environment.NewLine;
            }
            else
            {
                offset++;
            }
        }
        leaderboardScroller.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25 * highscoreList.Length);
        leaderboardContent.text = leaderboardContent.text.Replace('+', ' ');
        highscoreManager.loadingIndicator.gameObject.SetActive(false);
        //Debug.Log("displayed high scores");
    }

    IEnumerator RefreshHighscores()
    {
        while (true)
        {
            highscoreManager.DownloadHighscores();
            yield return new WaitForSeconds(30);
        }
    }

}
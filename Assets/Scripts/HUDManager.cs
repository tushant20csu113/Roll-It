using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject playScreen;
    public GameObject gameOverScreen;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverBestScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI playScreenBestScoreText;
    public TextMeshProUGUI diamondCountText;

    private int highScore;
    private int playerScore;
    private int diamondCount;
    private void OnEnable()
    {
        GameManager.OnGameEvent += OnGameEvent;
        Diamond.OnDiamondTaken += OnDiamondScore;
    }

   
    private void OnDisable()
    {
        GameManager.OnGameEvent -= OnGameEvent;
        Diamond.OnDiamondTaken -= OnDiamondScore;
    }
    private void Awake()
    {
        highScore = LoadHighScore();
        playerScore = 0;
        playScreenBestScoreText.text= String.Format("{0}", highScore);
        diamondCount=PlayerPrefs.GetInt("Diamonds");
        diamondCountText.text = String.Format("{0}", diamondCount);
    }

    private void OnGameEvent(eGameEvent gameEvent)
    {
        switch (gameEvent)
        {  
            case eGameEvent.GAME_OVER:
                ShowGameOver();
                break;
         
            case eGameEvent.GAME_START:
                ShowGameStart();
                break;
            case eGameEvent.GAME_SCORE:
                UpdateScore(1);
                break;
            default:
                break;
        }
    }
    public void SaveHighScore()
    { 
        PlayerPrefs.SetInt("HighScore", highScore);
 
    }
    public int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }
    private void ShowGameStart()
    {
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
        playScreen.SetActive(true);
        diamondCountText.text = String.Format("{0}", diamondCount);
    }
    public void ShowGameRestart()
    {
   
        highScore = LoadHighScore();
        playerScore = 0;
        UpdateScore(playerScore);
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(true);
        playScreen.SetActive(false);
    }
    private void ShowGameOver()
    {
        if(playerScore>highScore)
        {
            highScore = playerScore;
            SaveHighScore();
        }
        gameOverScreen.SetActive(true);
        menuScreen.SetActive(false);
        playScreen.SetActive(false);
        gameOverBestScoreText.text= String.Format("{0}",highScore);
        gameOverScoreText.text = String.Format("{0}", playerScore);
        PlayerPrefs.SetInt("Diamonds", diamondCount);
    }

    private void UpdateScore(int score)
    {
        if (score != null)
        { 
            playerScore += score;
            playScreen.GetComponentInChildren<TextMeshProUGUI>().text = String.Format("{0}", playerScore);
        }
    }
    private void OnDiamondScore()
    {
        diamondCount++;
        UpdateScore(1);
    }


}

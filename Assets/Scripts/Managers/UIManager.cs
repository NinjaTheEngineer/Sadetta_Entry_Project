using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI userInputText;

    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject playerHasHighscoreObject;
    [SerializeField] private GameObject gameOverScreen; 

    public static UIManager Instance;
    #endregion

    #region Monobehaviour
    private void Awake() //Simple singleton
    {
        Instance = this;
    }

    private void Start() //Initializes variables
    {
        gameOverScreen.GetComponent<Animator>().SetBool("Show", false);
        playerHasHighscoreObject.SetActive(false);
        GameManager.Instance.OnGameEnded += GameEnded;
    }

    private void OnDestroy() //Unsubscribes game ended event
    {
        GameManager.Instance.OnGameEnded -= GameEnded;
    }
    #endregion

    #region Methods
    private void GameEnded() //Show the gameover screen
    {
        gameOverScreen.GetComponent<Animator>().SetBool("Show", true);
    }

    public void SetScore(int score) //Set player's score
    {
        scoreText.text = "Score: " + score;
        highscoreText.text = "You scored " + score + " points!";
    }

    public void SaveUserScore() //Save user's Highscore with name
    {
        GameManager.Instance.SaveHighScore(userInputText.text);
    }

    public void PlayerGotHighscore(bool playerHasHighscore) //Handles if the player got top5 highscore
    {
        Debug.Log(">> set - " + playerHasHighscore);
        if (playerHasHighscore)
            playerHasHighscoreObject.SetActive(true);
        else
            playerHasHighscoreObject.SetActive(false);
    }

    public void ReturnToMainMenu() //Returns to main menu
    {
        LevelLoader.Instance.LoadMainMenu();
    }

    public void PlayAgain() //Restarts level
    {
        LevelLoader.Instance.RestartLevel();
    }
    #endregion
}

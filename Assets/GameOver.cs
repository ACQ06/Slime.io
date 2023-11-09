using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Game Over Score Text
    public TMP_Text gameOverScore;
    
    //Game Score Text
    public TMP_Text scoreValue;

    //Display Game Over Screen
    public void Setup()
    {
        
        gameObject.SetActive(true);
        gameOverScore.text = "Score: " + scoreValue.text;
    }

    //Play Again Button
    public void PlayAgain()
    {
        SceneManager.LoadScene("Endless");
    }


    //Main Menu Button
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

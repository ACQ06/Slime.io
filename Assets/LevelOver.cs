using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOver : MonoBehaviour
{
    public string sceneName;
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    //Play Again Button
    public void PlayAgain()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    //Main Menu Button
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}

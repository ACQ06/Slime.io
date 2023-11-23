using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    GameData data;

    private void Start()
    {
        data = SaveGame.LoadGameData();
    }

    public void Adventure()
    {
        if (data != null)
        {
            SceneManager.LoadSceneAsync(data.currentLevel, LoadSceneMode.Single);
        }


        else
        {
            SceneManager.LoadSceneAsync("Jungle", LoadSceneMode.Single);
        }
        
    }

    public void Endless()
    {
        SceneManager.LoadSceneAsync("Endless", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}

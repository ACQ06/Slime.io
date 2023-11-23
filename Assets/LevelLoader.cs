using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    string progress;
    public string LevelName;

    public Player player;
    public SwordAttack swordAttack;

    public TMP_Text levelProgressText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            progress = levelProgressText.text;

            if (progress == "100") {

                player.gameLevel = LevelName;
                SaveGame.SaveGameData(player, swordAttack);
                SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
            }
        }
    }


}

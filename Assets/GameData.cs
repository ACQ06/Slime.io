using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string currentLevel;
    public bool regen;
    public float playerSpeed;
    public float playerDamage;

    public GameData (Player player, SwordAttack swordAttack)
    {
        regen = player.Regen;
        playerSpeed = player.PlayerSpeed;
        playerDamage = swordAttack.damage;
        currentLevel = player.gameLevel;
    }
}

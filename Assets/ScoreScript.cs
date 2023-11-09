using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public static ScoreScript instance;

    private int killCount = 0;

    public TMP_Text score;

    private void Start()
    {
        score.text = killCount.ToString();
    }

    public void Update()
    {
        score.text = killCount.ToString();
    }

    public void Awake()
    {
        instance = this;
    }

    public void addKillCount()
    {
        killCount++;
    }
}

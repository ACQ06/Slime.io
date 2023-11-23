using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLevel2 : MonoBehaviour
{
    float initialEnemyCount;
    float remainingEnemyCount;

    public Image progress;
    public TMP_Text progressText;
    public TMP_Text levelProgressText;

    float levelProgress;

    // Start is called before the first frame update
    void Start()
    {
        initialEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        levelProgress = ((remainingEnemyCount / initialEnemyCount) * 100);
        levelProgressText.text = levelProgress.ToString();
        remainingEnemyCount = initialEnemyCount - GameObject.FindGameObjectsWithTag("Enemy").Length;
        progress.fillAmount = remainingEnemyCount / initialEnemyCount;
        progressText.text = "Progress: " + levelProgress.ToString("0.00") + "%";
    }
}

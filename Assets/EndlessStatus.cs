using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessStatus : MonoBehaviour
{
    private GameObject player;
    private GameObject[] enemies;
    private GameObject[] spawners;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            spawners = GameObject.FindGameObjectsWithTag("Spawner");
            for (int i = 0; i<spawners.Length; i++)
            {
                Destroy(spawners[i]);
            }

            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i<enemies.Length; i++)
            {
                Destroy(enemies[i]);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.increaseDamage(1);
                Destroy(gameObject);
            }
        }
    }
}

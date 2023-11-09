using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life_Regen : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Regen = true;
                Destroy(gameObject);
            }
        }
    }
}

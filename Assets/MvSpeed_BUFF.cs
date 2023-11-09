using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MvSpeed_BUFF : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if(player != null)
            {
                player.PlayerSpeed += player.PlayerSpeed * 0.20f;
                Destroy(gameObject);
            }
        }
    }
}

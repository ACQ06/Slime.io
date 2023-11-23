using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProperties : MonoBehaviour
{
    GameObject player;
    private Rigidbody2D rb;

    public float force = 5f;

    private float timer;

    public int damage = 1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        //damage += skeleton.damageIncrease;

        Vector3 direction = player.transform.position - transform.position;

        //Rotate projectiles to player's relative position (Cosine Function)
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotation-90);

        //Fire Projectiles
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
    }

    void Update()
    {
        //Remove projectile after a certain duration
        timer += Time.deltaTime;

        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    //Check if it hits a player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                //Decreased player health
                player.Health -= damage;

                //Remove Projectile
                Destroy(gameObject);
            }
        }
    }
}

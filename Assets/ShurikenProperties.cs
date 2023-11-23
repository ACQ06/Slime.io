using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenProperties : MonoBehaviour
{
    GameObject player;
    private Rigidbody2D rb;

    public float force = 3f;

    private float timer;

    private float timeBeforeAttack;

    public int damage = 3;

    Vector3 direction;

    bool Fired = false;

    public AudioSource sfx;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        direction = player.transform.position - transform.position;
    
    }

    void Update()
    {

        transform.Rotate(0, 0, 720 * Time.deltaTime); //rotates 50 degrees per second around z axis

        FireProjectile();

        //Remove projectile after a certain duration
        timer += Time.deltaTime;

        if (timer > 5)
        {
            Destroy(gameObject);
        }
    }

    void FireProjectile()
    {
        if (Fired)
        {
            return;
        }

        timeBeforeAttack += Time.deltaTime;
        if (timeBeforeAttack > 2)
        {
            sfx.Play();
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            Fired = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallProperties : MonoBehaviour
{
    GameObject player;
    private Rigidbody2D rb;

    public float force = 1f;

    private float timer;
    private float timeBeforeAttack;
    private bool Fired = false;

    Vector3 direction;

    public int damage = 1;

    public AudioSource sfx;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        direction = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        FireProjectile();

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
        if (timeBeforeAttack > 1.5)
        {
            sfx.Play();
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

            Fired = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();

            if(player != null)
            {
                player.Health -= damage;

                Destroy(gameObject);
            }
        }
    }
}

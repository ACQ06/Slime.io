using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSwordProperties : MonoBehaviour
{

    GameObject player;

    private Rigidbody2D rb;

    public float force = 2f;

    private float timer;

    private float timeBeforeAttack;

    public int damage = 3;

    bool Fired = false;

    Vector3 direction;

    public AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        direction = player.transform.position - transform.position;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotation + 135);
    }

    // Update is called once per frame
    void Update()
    {
        FireProjectile();
        timer += Time.deltaTime;
        if (timer > 5) Destroy(gameObject);
    }

    void FireProjectile()
    {
        if (Fired)
        {
            return;
        }

        timeBeforeAttack += Time.deltaTime;
        if (timeBeforeAttack > 3)
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

            if (player != null)
            {
                player.Health -= damage;

                Destroy(gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    private Transform player;
    public float speed;
    public float attackRadius = 0.5f;
    SpriteRenderer spriteRenderer;
    public bool canMove = false;

    public float cooldown = 3f; //Seconds
    private float lastAttackedAt = -9999f;

    public Transform firePoint;
    public GameObject arrowPrefab;

    [SerializeField]
    private AudioSource skeletonShoot;

    [SerializeField]
    private AudioSource skeletonDeath;

    public void PlayShootSFX()
    {
        if (gameObject != null)
        {
            skeletonShoot.Play();
        }
    }

    public void PlayDeathSFX()
    {
        if (gameObject != null)
        {
            skeletonDeath.Play();
        }
    }


    //Skeleton Health Properties
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0) {
                Defeated();
            }
        }

        get { return health; }
    }

    public float health = 1f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Defeated()
    {
        //Update player score count based on how many skeletons killed
        ScoreScript.instance.addKillCount();
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    public void Update()
    {
        Walk();
    }

    //Skeleton Walk Animation
    void Walk()
    {
        //Check if it can walk
        if (canMove)
        {
            bool inRadius = Fight();

            if (inRadius)
            {
                animator.SetBool("isWalking", false);
                if (Time.time > lastAttackedAt + cooldown)
                {
                    animator.SetBool("inRadius", true);
                }
            }

            else
            {
                animator.SetBool("inRadius", false);
                spriteRenderer.flipX = isPlayerInLeftSide();
                animator.SetBool("isWalking", true);
                //Walk towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
    }

    //Make Skeleton look on which side is the player
    private bool isPlayerInLeftSide()
    {
        if(transform.position.x - player.position.x > 0)
        {
            return true;
        }

        return false;
    }

    private bool Fight()
    {
        //CHECK IF PLAYER IS IN THE ATTACK RADIUS OF THE SKELETON
        if(Math.Pow((player.position.x - transform.position.x), 2) + Math.Pow((player.position.y - transform.position.y), 2) <= Math.Pow(attackRadius, 2))
        {
            return true;
        }
        return false;
    }

    public void LockMovement()
    {
        PlayShootSFX();
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    //Fire Projectiles
    public void Shoot()
    {
        //Play Attack Animation
        animator.SetBool("inRadius", false);

        //Generate Arrow Projectiles
        Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        
        lastAttackedAt = Time.time;
        
        UnlockMovement();
    }
}

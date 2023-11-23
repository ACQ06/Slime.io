using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    private Transform player;
    public float speed;
    public float attackRadius = 0.5f;
    SpriteRenderer spriteRenderer;

    public bool statue = false;

    bool canMove = true;

    public float cooldown = 3f; //Seconds
    private float lastAttackedAt = -9999f;

    public Transform firePoint;
    public GameObject arrowPrefab;

    [SerializeField]
    private AudioSource skeletonShoot;

    [SerializeField]
    private AudioSource skeletonDeath;

    public Image greenBar;

    private Material originalMaterial;

    [SerializeField] private Material flashMaterial;
    // The currently running coroutine.
    private Coroutine flashRoutine;
    [SerializeField] private float duration;

    public void Flash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    void UpdateHealthBar()
    {
        greenBar.fillAmount = health / maxHealth;
    }

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
            Flash();
            if (health <= 0) {
                Defeated();
            }
        }

        get { return health; }
    }

    public float health;
    float maxHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        healthBar.SetActive(true);
        AttachHealthBar();
        maxHealth = health;
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

    public GameObject healthBar;

    public Vector3 offset;


    public void AttachHealthBar()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Calculate health bar position in screen space
        Vector2 healthBarPosition = new Vector2(screenPosition.x + offset.x, screenPosition.y + offset.y);

        // Set health bar's position in screen space
        healthBar.transform.position = healthBarPosition;
    }

    public void Update()
    {
        Walk();
        UpdateHealthBar();
        AttachHealthBar();
    }

    //Skeleton Walk Animation
    void Walk()
    {
        if (health <= 0)
        {
            return;
        }

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
            if (statue)
            {
                return;
            }

            if (canMove)
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
        try
        {
            //CHECK IF PLAYER IS IN THE ATTACK RADIUS OF THE SKELETON
            if (Math.Pow((player.position.x - transform.position.x), 2) + Math.Pow((player.position.y - transform.position.y), 2) <= Math.Pow(attackRadius, 2))
            {
                return true;
            }
        }

        catch
        {
            
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
        canMove = !statue;
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

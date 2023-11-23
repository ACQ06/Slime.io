using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WolfScript : MonoBehaviour
{
    public AudioSource attackSfx;
    public AudioSource deathSfx;

    public float health;
    float maxHealth;
    Animator animator;
    private Transform player;
    SpriteRenderer spriteRenderer;
    public float speed;

    public float attackRadius = 0.1f;
    public float cooldown = 3f;
    private float lastAttackedAt = float.MinValue;

    bool canMove = true;

    private Material originalMaterial;
    [SerializeField] private Material flashMaterial;
    // The currently running coroutine.
    private Coroutine flashRoutine;
    [SerializeField] private float flashDuration = .01f;

    public Image greenBar;
    public GameObject healthBar;
    public Vector3 offset;

    public WolfAttack attack;

    public bool hasStation = false;
    float stationRadius = 1f;

    public float Health
    {
        set
        {
            health = value;
            Flash();
            if (health <= 0)
            {
                Defeated();
            }
        }

        get { return health; }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        healthBar.SetActive(true);
        AttachHealthBar();
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        AttachHealthBar();
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        greenBar.fillAmount = health / maxHealth;
    }

    public void AttachHealthBar()   
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // Calculate health bar position in screen space
        Vector2 healthBarPosition = new Vector2(screenPosition.x + offset.x, screenPosition.y + offset.y);

        // Set health bar's position in screen space
        healthBar.transform.position = healthBarPosition;
    }

    private bool PlayerInStation()
    {
        //CHECK IF PLAYER IS IN THE ATTACK RADIUS OF THE SKELETON
        if (Math.Pow((player.position.x - transform.position.x), 2) + Math.Pow((player.position.y - transform.position.y), 2) <= Math.Pow(stationRadius, 2))
        {
            return true;
        }
        return false;
    }

    private bool Fight()
    {
        //CHECK IF PLAYER IS IN THE ATTACK RADIUS OF THE SKELETON
        if (Math.Pow((player.position.x - transform.position.x), 2) + Math.Pow((player.position.y - transform.position.y), 2) <= Math.Pow(attackRadius, 2))
        {
            return true;
        }
        return false;
    }

    void Attack()
    {
        attackSfx.Play();
        if (isPlayerInLeftSide())
        {
            attack.AttackLeft();
        }

        else
        {
            attack.AttackRight();
        }
    }

    public void LockMovement()
    {
        canMove = false;
        Attack();
    }

    public void UnlockMovement()
    {
        attack.StopAttack();
        canMove = true;
        animator.SetBool("isAttacking", false);
        lastAttackedAt = Time.time;
    }


    void Walk()
    {
        if (health <= 0)
        {
            return;
        }

        bool inRadius = Fight();

        if (hasStation)
        {
            hasStation = !PlayerInStation();
            return;
        }
            
        if (inRadius)
        {
            animator.SetBool("isWalking", false);
            if (Time.time > lastAttackedAt + cooldown)
            {
                animator.SetBool("isAttacking", true);
            }
        }

        else
        {
            if (canMove)
            {
                animator.SetBool("isAttacking", false);
                spriteRenderer.flipX = isPlayerInLeftSide();
                animator.SetBool("isWalking", true);
                //Walk towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
        }
    }

    private bool isPlayerInLeftSide()
    {
        if (transform.position.x - player.position.x > 0)
        {
            return true;
        }

        return false;
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

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

    void Defeated()
    {
        deathSfx.Play();
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }
}

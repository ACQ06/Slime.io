using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    
    private Rigidbody2D rb;
    public float collisionOffset = 0.05f;
    public ContactFilter2D contactFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;
    public SwordAttack swordAttack;
    public TMP_Text healthText;

    public GameOver gameOver;

    public Image healthbar;

    bool canMove = true;

    [SerializeField]
    private AudioSource slimeJump;

    [SerializeField]
    private AudioSource swordSlash;


    public GameObject joystickButton;
    public GameObject attackButton;


    public bool Regen
    {
        set
        {
            regen = value;
        }

        get { return regen; }
    }

    //Player Health Properties
    public float Health
    {
        set
        {
            health = value;
        }

        get { return health; }
    }

    public float PlayerSpeed
    {
        set
        {
            playerSpeed = value;
        }

        get
        {
            return playerSpeed;
        }
    }

    //PLAYER PROPERTIES
    private float playerSpeed = 0.75f;
    public float maxHealth = 100f;
    private float health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    //Healthbar UI GREEN/RED
    void UpdateHealthBar()
    {
        healthbar.fillAmount = health / maxHealth;
    }

    void FixedUpdate()
    {
        LifeRegen();
        //Make health 0 if less than 0
        if (health < 0)
        {
            health = 0;
        }

        //Update health text on screen
        healthText.text = health.ToString() + "/" + maxHealth;
        UpdateHealthBar();

        //Check for player's current health
        if (health <= 0)
        {
            Defeated();
            return;
        }

        //Check if Move is Unlocked
        if (canMove )
        {
            //Check if Joystick is moving
            if (movementJoystick.joystickVector != Vector2.zero)
            {
                //Check if player can move in the direction
                bool success = TryMove(movementJoystick.joystickVector);

                //Slide player against location
                if (!success && movementJoystick.joystickVector.x > 0)
                {
                    success = TryMove(new Vector2(movementJoystick.joystickVector.x, 0));
                }

                if (!success && movementJoystick.joystickVector.y > 0)
                {
                    success = TryMove(new Vector2(0, movementJoystick.joystickVector.y));
                }
                //Play Walking Animation
                animator.SetBool("isMoving", success);
            }

            else
            {
                //Stop Walking Animation
                animator.SetBool("isMoving", false);
            }


            if (movementJoystick.joystickVector.x < 0)
            {
                spriteRenderer.flipX = true;

            }

            else if (movementJoystick.joystickVector.x > 0)
            {
                spriteRenderer.flipX = false;

            }
        }
    }

    //Make Player Move
    private bool TryMove(Vector2 direction)
    {
        //Move
        if(direction != Vector2.zero)
        {
            int count = rb.Cast(
                direction,
                contactFilter,
                castCollisions,
                playerSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * playerSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    //Play Player Attack Animation
    public void attack(){
        animator.SetTrigger("swordAttack");
    }

    public void increaseDamage(int damage)
    {
        swordAttack.damage += damage;
    }

    //Stop Player Attack Animation
    public void endAttack()
    {
        //Stop and Unlock Player Movement 
        swordAttack.stopAttack();
        UnlockMovement();
    }

    //Attack Animation Properties
    public void SwordAttack()
    {
        //Lock Movement Whenever Attacking
        LockMovement();

        swordSlash.Play();

        //Change Hitbox based off which side player is looking
        if (spriteRenderer.flipX == true)
        {
            swordAttack.attackLeft();
        }
            
        else
        {
            swordAttack.attackRight();
        }
    }

    //Play Player Death Animation
    void Defeated()
    {
        animator.SetTrigger("defeated");
    }


    //Game Over
    void EndGame()
    {
        joystickButton.SetActive(false);
        attackButton.SetActive(false);
        Destroy(gameObject);
        //Remove player from screen
        gameOver.Setup();
    }

    void playWalkSFX()
    {
        slimeJump.Play();
    }

    //Lock Player Movement
    void LockMovement()
    {
        canMove = false;
    }

    //Unlock Player Movement
    void UnlockMovement()
    {
        canMove = true;
    }


    bool regen = false;
    public float cooldown = 1f; //Seconds
    private float lastHealedAt = -9999f;

    //BUFFS
    public void LifeRegen()
    {
        if (regen)
        {
            if (Time.time > lastHealedAt + cooldown)
            {
                if(health < 50)
                {
                    health += 1f;
                    lastHealedAt = Time.time;
                }
            }
        }
    }
}

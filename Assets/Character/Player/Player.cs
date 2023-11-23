using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float collisionOffset = 0.05f;
    public ContactFilter2D contactFilter;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;
    public SwordAttack swordAttack;

    public TMP_Text healthText;

    public GameOver gameOver;

    public LevelOver levelOver;

    public Image healthbar;

    [SerializeField]
    private AudioSource slimeJump;

    [SerializeField]
    private AudioSource swordSlash;

    public GameObject mobileControls;

    public string gameLevel;

    public void SaveData()
    {
        SaveGame.SaveGameData(this, swordAttack);
    }

    public void LoadData()
    {
        GameData data = SaveGame.LoadGameData();

        if (data == null)
        {
            return;
        }

        Regen = data.regen;
        PlayerSpeed = data.playerSpeed;
        increaseDamage(data.playerDamage - 1);
        gameLevel = data.currentLevel;
    }

    public bool Regen
    {
        set
        {
            regen = value;
            Flash("Regen");
        }

        get { return regen; }
    }

    float previousHealth;
    //Player Health Properties
    public float Health
    {
        set
        {
            previousHealth = health;
            health = value;

            if (previousHealth > health)
            {
                Flash("Damage");
            }
        }

        get { return health; }
    }

    public float PlayerSpeed
    {
        set
        {
            playerSpeed = value;
            Flash("Movement");
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
        originalMaterial = spriteRenderer.material;

        LoadData();
    }

    //Healthbar UI GREEN/RED
    void UpdateHealthBar()
    {
        healthbar.fillAmount = health / maxHealth;
    }

    Vector2 joystick;


    void OnMove(InputValue movementValue)
    {
        joystick = movementValue.Get<Vector2>();
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
        //Check if Joystick is moving
        if (joystick != Vector2.zero)
        {
            //Check if player can move in the direction
            bool success = TryMove(joystick);

            //Slide player against location
            if (!success && joystick.x > 0)
            {
                success = TryMove(new Vector2(joystick.x, 0));
            }

            if (!success && joystick.y > 0)
            {
                success = TryMove(new Vector2(0, joystick.y));
            }
            //Play Walking Animation
            animator.SetBool("isMoving", success);
        }

        else
        {
            //Stop Walking Animation
            animator.SetBool("isMoving", false);
        }


        if (joystick.x < 0)
        {
            spriteRenderer.flipX = true;

        }

        else if (joystick.x > 0)
        {
            spriteRenderer.flipX = false;

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
    public void OnAttack(){
        animator.SetBool("isAttacking", true);
    }

    public void increaseDamage(float damage)
    {
        swordAttack.damage += damage;
        Flash("Damage Increase");
    }

    //Stop Player Attack Animation
    public void endAttack()
    {
        swordAttack.stopAttack();
        animator.SetBool("isAttacking", false);
    }

    //Attack Animation Properties
    public void SwordAttack()
    {
        swordSlash.Play();

        print("ATTACK");

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
        print("END GAME");
        mobileControls.SetActive(false);


        Destroy(gameObject);

        if (gameOver != null)
        {
            gameOver.Setup();
        }

        else
        {
            levelOver.Setup();
        }
    }

    void playWalkSFX()
    {
        slimeJump.Play();
    }


    bool regen = false;
    public float regenCooldown = 1f; //Seconds
    private float lastHealedAt = -9999f;

    //BUFFS
    public void LifeRegen()
    {
        if (regen)
        {
            if (Time.time > lastHealedAt + regenCooldown)
            {
                if(health < maxHealth)
                {
                    health += 1f;
                    lastHealedAt = Time.time;
                }
            }
        }
    }

    private Material originalMaterial;
    [SerializeField] private Material damageFlash;
    [SerializeField] private Material regenFlash;
    [SerializeField] private Material movementFlash;
    [SerializeField] private Material damageIncreaseFlash;


    // The currently running coroutine.
    private Coroutine flashRoutine;
    [SerializeField] private float flashDuration = .01f;

    private IEnumerator FlashRoutine(string FlashIndicator)
    {

        if (FlashIndicator == "Damage")
        {
            spriteRenderer.material = damageFlash;
        }

        else if (FlashIndicator == "Regen")
        {
            spriteRenderer.material = regenFlash;
        }

        else if (FlashIndicator == "Movement")
        {
            spriteRenderer.material = movementFlash;
        }

        else if (FlashIndicator == "Damage Increase")
        {
            spriteRenderer.material = damageIncreaseFlash;
        }

        // Swap to the flashMaterial.
        

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(flashDuration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    public void Flash(string FlashIndicator)
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine(FlashIndicator));
    }
}

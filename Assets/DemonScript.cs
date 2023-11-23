using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DemonScript : MonoBehaviour
{

    public AudioSource attackSfx;
    public AudioSource deathSfx;

    //Demon Properties
    private float health = 500f;
    Animator animator;

    float speed = 0.5f;

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

    bool canMove = true;
    float attackRadius = 1.5f;

    //Health Bar
    public Image healthBar;
    public TMP_Text healthText;

    float maxDemonHealth;

    //Projectiles
    public GameObject swordPrefab;
    public GameObject shurikenPrefab;
    public GameObject fireballPrefab;

    //ENTITIES
    public GameObject skeletonPrefab;
    public GameObject wolfPrefab;

    //Player's Position
    private Transform playerTransform;


    public float cooldown = 1.25f; //Seconds
    private float lastAttackedAt = -9999f;

    //Flash
    private Material originalMaterial;
    [SerializeField] private Material flashMaterial;
    // The currently running coroutine.
    private Coroutine flashRoutine;
    [SerializeField] private float flashDuration = .01f;
    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        maxDemonHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        if (health <= 0)
        {
            health = 0;
        }

        healthBar.fillAmount = health / maxDemonHealth;
        healthText.text = health.ToString() + "/" + maxDemonHealth;
    }

    bool InRadius()
    {
        try
        {
            //CHECK IF PLAYER IS IN THE ATTACK RADIUS OF THE SKELETON
            if (Math.Pow((playerTransform.position.x - transform.position.x), 2) + Math.Pow((playerTransform.position.y - transform.position.y), 2) <= Math.Pow(attackRadius, 2))
            {
                return true;
            }
        }

        catch
        {

        }

        return false;
    }

    void Walk()
    {
        if (health <= 0)
        {
            return;
        }

        bool inRadius = InRadius();

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
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
        }
    }

    private bool isPlayerInLeftSide()
    {
        if (transform.position.x - playerTransform.position.x > 0)
        {
            return true;
        }

        return false;
    }

    void stopAttack()
    {
        canMove = true;
    }

    void Attack()
    {
        
        canMove = false;
        if (Time.time > lastAttackedAt + cooldown)
        {
            attackSfx.Play();
            int randomNumber = UnityEngine.Random.Range(0, 5);
            switch (randomNumber)
            {
                case 0:
                    FireBallAttack();
                    break;

                case 1:
                    ShurikenAttack();
                    break;

                case 2:
                    SwordAttack();
                    break;

                case 3:
                    SpawnWolfAttack();
                    break;

                case 4:
                    SpawnSkeletonAttack();
                    break;
            }

            lastAttackedAt = Time.time;
        }
    }

    void FireBallAttack()
    {
        int projectilesCount = 8;
        float projectilesDistance = 1.5f;

        int randomNumber = UnityEngine.Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                SpawnProjectilesAroundPlayer(fireballPrefab, projectilesCount, projectilesDistance);
                break;

            case 1:
                SpawnProjectilesAroundDemon(fireballPrefab, projectilesCount, projectilesDistance);
                break;

            case 2:
                SpawnProjectilesBesidePlayer(fireballPrefab, projectilesCount, projectilesDistance);
                break;
        }
    }

    void ShurikenAttack()
    {
        int projectilesCount = 8;
        float projectilesDistance = 1.5f;

        int randomNumber = UnityEngine.Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                SpawnProjectilesAroundPlayer(shurikenPrefab, projectilesCount, projectilesDistance);
                break;

            case 1:
                SpawnProjectilesAroundDemon(shurikenPrefab, projectilesCount, projectilesDistance);
                break;

            case 2:
                SpawnProjectilesBesidePlayer(shurikenPrefab, projectilesCount, projectilesDistance);
                break;
        }

    }

    void SwordAttack()
    {
        int projectilesCount = 8;
        float projectilesDistance = 1.5f;

        int randomNumber = UnityEngine.Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                SpawnProjectilesAroundPlayer(swordPrefab, projectilesCount, projectilesDistance);
                break;

            case 1:
                SpawnProjectilesAroundDemon(swordPrefab, projectilesCount, projectilesDistance);
                break;

            case 2:
                SpawnProjectilesBesidePlayer(swordPrefab, projectilesCount, projectilesDistance);
                break;
        }

    }

    void SpawnWolfAttack()
    {
        int wolfCount = 2;
        float wolfDistance = 1.5f;

        int randomNumber = UnityEngine.Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                SpawnProjectilesAroundPlayer(wolfPrefab, wolfCount, wolfDistance);
                break;

            case 1:
                SpawnProjectilesAroundDemon(wolfPrefab, wolfCount, wolfDistance);
                break;

            case 2:
                SpawnProjectilesBesidePlayer(wolfPrefab, wolfCount, wolfDistance);
                break;
        }
    }

    void SpawnSkeletonAttack()
    {
        int skeletonCount = 2;
        float skeletonDistance = 1.5f;

        int randomNumber = UnityEngine.Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                SpawnProjectilesAroundPlayer(skeletonPrefab, skeletonCount, skeletonDistance);
                break;

            case 1:
                SpawnProjectilesAroundDemon(skeletonPrefab, skeletonCount, skeletonDistance);
                break;

            case 2:
                SpawnProjectilesBesidePlayer(skeletonPrefab, skeletonCount, skeletonDistance);
                break;
        }
    }


    private void SpawnProjectilesAroundPlayer(GameObject objectToSpawn, int numberOfObjects, float spawnRadius)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * 2 * Mathf.PI / numberOfObjects; 
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius + playerTransform.position;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnProjectilesAroundDemon(GameObject objectToSpawn, int numberOfObjects, float spawnRadius)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * 2 * Mathf.PI / numberOfObjects;
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius + transform.position;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private void SpawnProjectilesBesidePlayer(GameObject objectToSpawn, int numberOfObjects, float distance)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            // Use Mathf.PI / 2 to spawn objects on the right side
            // Use 3 * Mathf.PI / 2 to spawn objects on the left side
            float angle = Mathf.PI / 2 * (i % 2 == 0 ? 1 : -1);

            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance + transform.position;
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
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
        Destroy(gameObject);
        SceneManager.LoadScene("Ending", LoadSceneMode.Single);
    }
}
    
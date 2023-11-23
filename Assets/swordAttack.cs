using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;
    public float damage = 1;
    

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    //Switch hitbox position
    public void attackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
        print("Attack Right");
    }

    public void attackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        print("Attack Left");
    }

    public void stopAttack()
    {
        swordCollider.enabled = false;
    }


    //Check if it hits the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {

            if (collision.name.Contains("Wolf"))
            {
                WolfScript wolf = collision.GetComponent<WolfScript>();
                if (wolf != null)
                {
                    //decrease enemy health
                    wolf.Health -= damage;
                }
            }

            else if (collision.name.Contains("Skeleton"))
            {
                EnemyScript skeleton = collision.GetComponent<EnemyScript>();
                if (skeleton != null)
                {
                    //decrease enemy health
                    skeleton.Health -= damage;
                }
            }

            else if (collision.name.Contains("Demon"))
            {
                DemonScript demon = collision.GetComponent<DemonScript>();

                if(demon != null)
                {
                    demon.Health -= damage;
                }
            }
        }
    }
}

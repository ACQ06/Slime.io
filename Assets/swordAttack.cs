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
        rightAttackOffset = transform.position;
    }

    //Switch hitbox position
    public void attackRight()
    {
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void attackLeft()
    {
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
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
            EnemyScript enemy = collision.GetComponent<EnemyScript>();

            if (enemy != null ){
                //decrease enemy health
                enemy.Health -= damage;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttack : MonoBehaviour
{
    Vector2 rightAttackOffset;
    public Collider2D attackHitbox;
    public float damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    public void AttackLeft()
    {   
        attackHitbox.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        print("LEFT");
    }

    public void AttackRight()
    {
        attackHitbox.enabled = true;
        transform.localPosition = rightAttackOffset;
        print("RIGHT");
    }

    public void StopAttack()
    {
        attackHitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                //Decreased player health
                player.Health -= damage;
            }
        }
    }
}

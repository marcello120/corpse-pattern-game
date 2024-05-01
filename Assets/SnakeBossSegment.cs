using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeBossSegment : EnemyHitbox
{
    public float health;
    public SnakeBoss snakeBoss;
    public Animator animator;
    public GameObject spawnOnDeath;
    public GameObject hiteffect;

    public void setup(SnakeBoss enemy, float health)
    {
        snakeBoss = enemy;
        this.health = health;
        Init();
    }


    public override void Init()
    {
        parentEnemy = snakeBoss;
        animator = GetComponent<Animator>();
    }

    public void Kill(GameObject toSpawn)
    {
        spawnOnDeath = toSpawn;
        animator.SetTrigger("Die");
    }

    public void Die()
    {
        if(spawnOnDeath != null)
        {
            Instantiate(spawnOnDeath, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public override void getHit(float damage, Vector2 knockbac, Vector3 direction)
    {
        health -= damage;
        if(health <= 0)
        {
            snakeBoss.segmentDestroyed(this);
            return;
        }
        else if(animator!= null)
        {
            animator.SetTrigger("Hit");
        }
        Instantiate(hiteffect, transform.position, Quaternion.identity);
        snakeBoss.getHitSegment(damage, this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.GetComponent<SnakeBossSegment>()!=null)
        {
            return;
        }
        animator.SetTrigger("Dmg");
        snakeBoss.damagePlayer(collision.collider);
    }
}

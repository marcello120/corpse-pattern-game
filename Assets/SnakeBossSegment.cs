using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeBossSegment : EnemyHitbox
{
    public float health;
    public SnakeBoss snakeBoss;

    public void setup(SnakeBoss enemy, float health)
    {
        snakeBoss = enemy;
        this.health = health;
        Init();
    }


    public override void Init()
    {
        parentEnemy = snakeBoss;
    }

    public override void getHit(float damage, Vector2 knockbac, Vector3 direction)
    {
        //TODO: playhit anim
        //TODO: spwan hit effect


        health -= damage;
        if(health <= 0)
        {
            snakeBoss.segmentDestroyed(this);
        }
        snakeBoss.getHitSegment(damage, this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //TODO: play damage playe anim

        if (collision.collider.GetComponent<SnakeBossSegment>()!=null)
        {
            return;
        }
        snakeBoss.damagePlayer(collision.collider);
    }
}

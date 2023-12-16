using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    public string enemyname;

    private GameManager gameManager;

    public Animator animator;

    public Collider2D boxCollider2D;

    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;

    public AudioSource getHitSound;

    public int flipBehaviour;


    public GameObject corpse;

    public float health;
    public bool isDead = false;
    public bool isMoving = false;
    public float attackPower;
    public float movemetSpeed;

    public int corpseNumber = 1;

    public Boolean stunned;
    public float stunTimer;
    public float stunTime;


    // Should be called in Start
    public virtual void Init()
    {
        //this should be done with events
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        getHitSound = GetComponent<AudioSource>();
    }


    public virtual void Death()
    {

        Vector3 place = gameManager.AddWorldPosToGridAndReturnAdjustedPos(transform.position,corpseNumber);

        animator.SetTrigger("Death");
        boxCollider2D.enabled = false;

        if (corpse!=null)
        {
            Instantiate(corpse, place, Quaternion.identity);

        }
        else
        {
            Debug.LogError("Corspe must be set");
        }
        isDead = true;


    }


    public virtual void getHit(float damage, Vector2 knockback)
    {
        if(getHitSound!= null)
        {
            getHitSound.Play();
        }


        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        else
        {
            rb.AddForce(knockback);
            animator.SetTrigger("Hit");
        }
    }

    public virtual void Remove()
    {
        Destroy(gameObject);
    }


    public void damagePlayer(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RiggedPlayerController playerController = collision.gameObject.GetComponent<RiggedPlayerController>();
            playerController.takeDamage(attackPower, this);
        }
    }

    public bool moveToPlayerWithDetectionZone(DetectionZoneController detectionZoneController)
    {
        isMoving = false;

        if (detectionZoneController.detectedObjs.Count > 0)
        {
            if (detectionZoneController.detectedObjs[0] != null && health > 0)
            {
                isMoving = true;
                GameObject target = detectionZoneController.detectedObjs[0];

                Vector2 directionToTarget = (target.transform.position - transform.position).normalized;


                handleFlip(flipBehaviour, directionToTarget);

                rb.AddForce(directionToTarget * movemetSpeed);

                isMoving = true;
            }
        }
        animator.SetBool("IsMoving", isMoving);

        return isMoving;
    }

    public virtual bool stun()
    {
        Debug.Log("Abstract Stun");
        stunned = true;
        return stunned;
    }

    public void handleFlip(int flipBehaviour, Vector3 directionToTarget)
    {
        if(flipBehaviour != -1 && flipBehaviour != 1)
        {
            return;
        }
        if (flipBehaviour == 1)
        {
            if (directionToTarget.x < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        if (flipBehaviour == -1)
        {
            if (directionToTarget.x > 0f)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(StatusHolder))]

public abstract class Enemy : MonoBehaviour, IEnemy
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;
    protected AudioSource getHitSound;


    [Header("Set these")]
    public GameObject body;
    public string enemyname;
    public GameObject corpse;
    public int corpseNumber = 1;
    public int flipBehaviour;
    public NearDeathStatusEffect nearDeathStatusEffect;

    [Header("Stats")]
    public float health;
    public float attackPower;
    public float movemetSpeed;


    [Header("Debug")]
    public State state = State.Idle;
    public bool isDead = false;
    public bool stunned;


    [Header("Extra")]
    public string extra;
    public Transform target;
    public StatusHolder statusHolder;



    public enum State
    {
        Idle,
        Waiting,
        Moving,
        Attacking,
        Dead
    }

    // Should be called in Start
    public virtual void Init()
    {
        //this should be done with events
        animator = body.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer= body.GetComponent<SpriteRenderer>();
        getHitSound = GetComponent<AudioSource>();
        TryGetComponent(out statusHolder);
    }


    public virtual void Death()
    {
        rb.velocity = Vector3.zero;


        Vector3 place = GameManager.Instance.AddWorldPosToGridAndReturnAdjustedPos(transform.position,corpseNumber);

        animator.SetTrigger("Death");
        Collider2D[] ccs = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D cc in ccs)
        {
            cc.enabled = false;
        }
    
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

        if (health - damage <= 0)
        {
            addStatusEffect(Instantiate(nearDeathStatusEffect));
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

    public bool isStunned()
    {
        if(statusHolder == null)
        {
            return false;
        }
        if (statusHolder.effects.Any(x => x!=null && x.statusEffectName=="Stun"))
        {
            return true;
        }
        return false;
    }

    public bool canMove()
    {
        return (isDead || state == State.Dead || isStunned());
    }

    public void addStatusEffect(StatusEffect statusEffect)
    {
        if (statusHolder != null & statusEffect != null)
        {
            statusHolder.Add(statusEffect);
        }
        else
        {
            Debug.LogError("NO EFFECT || NO STATUS FUCK YOU!");
        }
    }

    public void moveInDirection(Vector3 direction)
    { 
        if(!isStunned()) { 
        Vector3 force = direction * movemetSpeed * Time.fixedDeltaTime;
        rb.AddForce(force);
        }
    }

    public bool moveToPlayerWithDetectionZone(DetectionZoneController detectionZoneController)
    {
        //isMoving = false;

        //if (detectionZoneController.detectedObjs.Count > 0)
        //{
        //    if (detectionZoneController.detectedObjs[0] != null && health > 0)
        //    {
        //        isMoving = true;
        //        GameObject target = detectionZoneController.detectedObjs[0];

        //        Vector2 directionToTarget = (target.transform.position - transform.position).normalized;


        //        handleFlip(flipBehaviour, directionToTarget);

        //        rb.AddForce(directionToTarget * movemetSpeed);

        //        isMoving = true;
        //    }
        //}
        //animator.SetBool("IsMoving", isMoving);

        //return isMoving;
        return true;
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
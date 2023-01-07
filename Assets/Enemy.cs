using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    private GameManager gameManager;

    public string enemyname;

    public Animator animator;

    public BoxCollider2D boxCollider2D;

    public Rigidbody2D rb;

    public GameObject copse;

    public float health;
    public bool isDead;



    // Start is called before the first frame update
    public void Init()
    {
        //this should be done with events
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Debug.Log("SUPER START");
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame


    public void Death()
    {

        Vector3 place = gameManager.AddWorldPosToGridAndReturnAdjustedPos(transform.position);

        Instantiate(copse, place, Quaternion.identity);
        animator.SetTrigger("Death");
        boxCollider2D.enabled = false;
    }


    public void getHit(float damage, Vector2 knockback)
    {
        rb.AddForce(knockback);

        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}

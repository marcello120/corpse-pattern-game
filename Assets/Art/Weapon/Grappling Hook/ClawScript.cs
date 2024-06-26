using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawScript : MonoBehaviour
{
    public GameObject Grappler;
    public LassoScript lasszo;
    public bool attached;
    public bool isShot;
    private Transform targetHit;

    void Start()
    {
        lasszo = Grappler.GetComponent<LassoScript>();
        attached = false;
        isShot = false;
    }

    void Update()
    {
        if (attached && targetHit != null)
        {
            // Update the bullet's position based on the attached target
            transform.position = targetHit.position;
        }
        if(!attached && !isShot)
        {
            transform.position = transform.parent.position;
        }
    }

    public void Shot()
    {
        isShot = true;
    }
    public void NotShot()
    {
        isShot = false;
    }
    public void NotAttached()
    {
        attached = false;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called");
        Debug.Log(other.gameObject.name);
        Debug.Log(other.tag);

        if (!attached && isShot) // Check if the claw is shot and not attached
        {
            if (other.CompareTag("Enemy"))
            {
                // Attach the bullet to the enemy
                AttachToEnemy(other.transform);

                Enemy enemyScript = other.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Set the enemy as stunned
                    enemyScript.isStunned();
                }
            }
            else if (other.CompareTag("Wall"))
            {
                // Bounce off walls
                lasszo.RecallClaw();
            }
            else
            {
                // Attach the bullet to the enemy
                //AttachToEnemy(other.transform);
            }
        }
    }

    private void AttachToEnemy(Transform enemyTransform)
    {
        Debug.Log("AttachToEnemy called");
        // Disable physics interactions
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
        // Set the targetHit to the enemy's transform
        targetHit = enemyTransform;

        // Set attached to true
        attached = true;
        lasszo.Attach();
        lasszo.NotMoving();
        isShot = false;

        // Optionally, disable the collider to prevent further collisions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        // Parent the bullet to the enemy
        //transform.parent = enemyTransform;

        //// Reset local position to zero
        //transform.localPosition = Vector3.zero;

    }
}

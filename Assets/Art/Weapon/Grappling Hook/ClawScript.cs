using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawScript : MonoBehaviour
{
    public LassoScript lasszo;
    public bool attached;
    private Transform targetHit;

    void Start()
    {
        lasszo = GetComponent<LassoScript>();
        attached = false;
    }

    void Update()
    {
        if (attached && targetHit != null)
        {
            // Update the bullet's position based on the attached target
            transform.position = targetHit.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!(other.gameObject.tag == "Player") || !(other.gameObject.layer == 10))
        {
            return;
        }
        if (!attached)
        {
            if (other.CompareTag("Enemy") && !attached)
            {
                Enemy enemyScript = other.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    // Set the enemy as stunned
                    enemyScript.isStunned();
                }
            }
            // Attach the bullet to the enemy
            AttachToEnemy(other.transform);
        }
    }

    private void AttachToEnemy(Transform enemyTransform)
    {
        // Disable physics interactions
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        // Parent the bullet to the enemy
        transform.parent = enemyTransform;

        // Reset local position to zero
        transform.localPosition = Vector3.zero;

        // Set the targetHit to the enemy's transform
        targetHit = enemyTransform;

        // Set attached to true
        attached = true;
        lasszo.Attach();

        // Optionally, disable the collider to prevent further collisions
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
    
}

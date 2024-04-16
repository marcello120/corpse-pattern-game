using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSand_Pull : MonoBehaviour
{
    public float maxPullStrength = 100f; // Maximum strength of the pull
    public float swirlSpeed = 3f; // Speed of the swirling motion

    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("EnemyBody")) || other.CompareTag("Player"))
        {
            // Check if the colliding object has a Rigidbody2D
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction towards the center of the quicksand
                Vector2 direction = (transform.position - other.transform.position).normalized;

                // Calculate distance from the center
                float distance = Vector2.Distance(transform.position, other.transform.position);

                // Calculate pull strength based on distance
                float pullStrength = Mathf.Clamp(1 - distance / GetComponent<CircleCollider2D>().radius, 0f, 1f);

                // Calculate normalized swirl speed based on distance
                float normalizedSwirlSpeed = Mathf.Clamp(1 - GetComponent<CircleCollider2D>().radius / distance, 0f, 1f);

                // Calculate swirling motion in the opposite direction
                Vector2 swirl = new Vector2(direction.y, -direction.x) * swirlSpeed * normalizedSwirlSpeed;

                // Apply force towards the center with swirling motion and variable strength
                rb.AddForce((direction + swirl) * maxPullStrength * (pullStrength + 1f) * Time.deltaTime);
            }
        }
    }
}

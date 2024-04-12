using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit = false;
    private Quaternion stoppedRotation;
    private float timeSinceStart = 0f;
    private float timeSinceHit = 0f;
    public float timeAfterFadeOut = 10f;
    public float fadeOutSpeed = 3f;
    public float attackPower = 1f;
    public float knockbackPower;
    public float speed;
    public GameObject impactParticles;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!hasHit) //Ha ki van love, de nem talalt semmit
        {
            StartCoroutine(CalculateSpeed());

            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            stoppedRotation =  Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = stoppedRotation;

            timeSinceStart += Time.deltaTime;

            if (timeSinceStart >= 3f)
            {
                StopArrowMovement();
            }
        }
        else if(hasHit) //Ha eltalalt valamit, egy ido utan tunjon el?
        {
            timeSinceHit += Time.deltaTime;
            if(timeSinceHit>= timeAfterFadeOut)
            {
                Color objectColor = this.GetComponent<Renderer>().material.color;
                float fadeAmount = objectColor.a - (fadeOutSpeed*Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                this.GetComponent<Renderer>().material.color = objectColor;
                Destroy(gameObject, 3);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasHit)
        {
            if (col.CompareTag("Enemy") && col.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
            {
                hasHit = true;

                //// Disable the collider to prevent further collisions
                //Collider2D collider = GetComponent<Collider2D>();
                //if (collider != null)
                //{
                //    collider.enabled = false;
                //}

                // Calculate the midpoint between arrow's current position and enemy's position & Set the arrow's position to the midpoint
                Vector3 midpoint = (transform.position + col.transform.position) * 0.5f;
                transform.position = midpoint;

                transform.parent = col.transform;

                stoppedRotation = transform.rotation;


                if (col.GetComponent<EnemyHitbox>()!= null)
                {
                    Vector3 directionToEnemy = (col.gameObject.transform.position - transform.parent.parent.position).normalized;
                    col.GetComponent<EnemyHitbox>().getHit(attackPower, GetKnockBack(col), directionToEnemy); //A hit effect itt torolheto, nem?
                    Vector2 contactpoint = col.ClosestPoint(transform.position);

                    /*if (hitEffect != null)
                    {
                       Instantiate(hitEffect, contactpoint, Quaternion.identity);
                    }*/
                }

                // Disable Rigidbody to prevent unwanted physics interactions
                if (rb != null)
                {
                    Destroy(rb);
                }
            }
            else if (col.CompareTag("Wall"))
            {
                hasHit = true;

                // Disable the collider to prevent further collisions
                Collider2D collider = GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Set the arrow's parent to the enemy
                Transform enemyTransform = col.transform;
                transform.parent = enemyTransform;
                stoppedRotation = transform.rotation;

                SpawnImpactParticles(transform.position, col.transform.position);

                // Disable Rigidbody to prevent unwanted physics interactions
                if (rb != null)
                {
                    Destroy(rb);
                }
            }
            if (col.tag == "Destructible")
            {
                hasHit = true;

                Collider2D collider = GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Set the arrow's parent to the enemy
                Transform enemyTransform = col.transform;
                transform.parent = enemyTransform;
                stoppedRotation = transform.rotation;

                // Disable Rigidbody to prevent unwanted physics interactions
                if (rb != null)
                {
                    Destroy(rb);
                }

                Vector2 contactpoint = col.ClosestPoint(transform.position);
                //if (hitEffect != null)
                //{
                //    Instantiate(hitEffect, contactpoint, Quaternion.identity);
                //}
                Destructible destructible = col.GetComponent<Destructible>();
                if (destructible != null)
                {
                    destructible.hitDestructible(attackPower);
                }
            }
        }
    }

    private void StopArrowMovement()
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        transform.rotation = stoppedRotation;

        // Disable the collider to prevent further collisions
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    private Vector2 GetKnockBack(Collider2D collision)
    {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = parentPosition - collision.gameObject.transform.position;

        return (direction.normalized * knockbackPower * speed * -1);
    }

    IEnumerator CalculateSpeed()
    {
        Vector3 lastPos = transform.position;
        yield return new WaitForFixedUpdate();
        speed = (lastPos - transform.position).magnitude / Time.deltaTime;
    }

    public void SpawnImpactParticles(Vector3 spawnPos, Vector3 direction)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);
        Instantiate(impactParticles, spawnPos, rotation);
    }
}

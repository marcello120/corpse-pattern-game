using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!hasHit)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!hasHit)
        {
            if (col.CompareTag("Enemy"))
            {
                hasHit = true;

                // Disable the collider to prevent further collisions
                Collider2D collider = GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Calculate the midpoint between arrow's current position and enemy's position
                Vector3 midpoint = (transform.position + col.transform.position) * 0.5f;

                // Set the arrow's position to the midpoint
                transform.position = midpoint;

                // Set the arrow's parent to the enemy
                transform.parent = col.transform;

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

                // felezzük a távot a becsapódás és az enemy között

                // Disable Rigidbody to prevent unwanted physics interactions
                if (rb != null)
                {
                    Destroy(rb);
                }
            }
        }
    }
   
    /*Enemy enemy = collision.GetComponent<Enemy>();
   if(collision.GetComponent<EnemyHitbox>()!= null)
            {
                Vector3 directionToEnemy = (collision.gameObject.transform.position - transform.parent.parent.position).normalized;
    collision.GetComponent<EnemyHitbox>().getHit(weaponAttackPower, GetKnockBack(collision), directionToEnemy);
    Vector2 contactpoint = collision.ClosestPoint(transform.position);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, contactpoint, Quaternion.identity);
}
            }*/
 }

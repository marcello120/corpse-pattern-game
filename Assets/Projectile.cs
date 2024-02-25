using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public bool isParried = false;

    public void Setup(Vector3 direction, float damage)
    {
        this.damage = damage;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        Destroy(gameObject, 5f);
    }

    public void Parried()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        Vector3 originalVelocity = rigidbody.velocity;
        rigidbody.velocity= Vector3.zero;
        rigidbody.AddForce(originalVelocity*-1.5f,ForceMode2D.Impulse);
        float angle = Mathf.Atan2(-originalVelocity.y, -originalVelocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
        isParried = true;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") { 
            RiggedPlayerController player = collision.GetComponent<RiggedPlayerController>();
            if(player != null)
            {
                player.takeDamage(damage, this.gameObject);
                Destroy(gameObject);
            }
        }
        if(collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (collision.tag == "Enemy" && isParried && collision.gameObject.layer == LayerMask.NameToLayer("EnemyBody"))
        {
            EnemyHitbox hitbox = collision.gameObject.GetComponent<EnemyHitbox>();
            if(hitbox != null)
            {
                hitbox.getHit(damage, GetKnockBack(collision));
            }
            Destroy(gameObject);

        }
    }

    private Vector2 GetKnockBack(Collider2D collision)
    {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = parentPosition - collision.gameObject.transform.position;

        return (direction.normalized * 1 * -1);
    }

}

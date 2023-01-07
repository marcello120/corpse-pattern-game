using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : Weapon
{
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider2;
    private Animator animator;

    public float weaponAttackPower = 10;
    public float weaponKnockback = 10;
    public float speed = 10;


    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        polygonCollider2 = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();

        polygonCollider2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        polygonCollider2.enabled= true;
    }

    public void EndAttack()
    {
        polygonCollider2.enabled = false;

    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 knockback = GetKnockBack(collision);
                enemy.getHit(weaponAttackPower, knockback);
            }
        }
    }

    private Vector2 GetKnockBack(Collider2D collision)
    {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = parentPosition - collision.gameObject.transform.position;

        return (direction.normalized * weaponKnockback * -1);
    }
}

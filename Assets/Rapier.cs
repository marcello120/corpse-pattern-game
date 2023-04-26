using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapier : Weapon
{
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider2;
    private Animator animator;
    private AudioSource hitSound;

    public float weaponAttackPower = 10;
    public float weaponKnockback = 10;

    public float speed = 10;
    public bool attackQueued = false;
    public float timeSinceAttack = 0;
    public bool canAttack = true;


    private bool attackState = false;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        polygonCollider2 = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
        hitSound = GetComponent<AudioSource>();

        polygonCollider2.enabled = false;
    }

    void FixedUpdate()
    {
        if (!canAttack)
        {
            timeSinceAttack += Time.deltaTime;
            if (timeSinceAttack > speed)
            {
                timeSinceAttack = 0;
                canAttack = true;
                if (attackQueued)
                {
                    Attack();
                }
            }
        }
    }

    public void StartAttack()
    {
        polygonCollider2.enabled= true;
        attackState= true;
    }

    public void EndAttack()
    {
        polygonCollider2.enabled = false;
        attackState = false;
        canAttack = false;

    }

    public override void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");
            hitSound.Play();
            attackQueued = false;

        }
        else
        {
            attackQueued = true;
        }
        
    }


    public override bool isAttacking()
    {
        return attackState;
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

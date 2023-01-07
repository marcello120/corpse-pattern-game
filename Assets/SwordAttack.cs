using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    BoxCollider2D swordCollider;
    Vector2 attackOffset;
    public float attackpower = 10;
    public float knockbackPower = 500;


    // Start is called before the first frame update
    void Start()
    {
        swordCollider = GetComponent<BoxCollider2D>();
        swordCollider.enabled = false;
        attackOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackRight()
    {
        swordCollider.enabled = true;
        Debug.Log(transform.position);
        transform.localPosition = new Vector2(Mathf.Abs(transform.localPosition.x), transform.localPosition.y);

    }

    public void AttackLeft()
    {
        swordCollider.enabled = true;
        Debug.Log(transform.position);
        transform.localPosition = new Vector2 (-Mathf.Abs(transform.localPosition.x), transform.localPosition.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 knockback = GetKnockBack(collision);
                enemy.getHit(attackpower,knockback);
            }
        }
    }

    private Vector2 GetKnockBack(Collider2D collision)
    {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

        Vector2 direction = parentPosition - collision.gameObject.transform.position;

        return (direction.normalized * knockbackPower *-1);
    }
}
       

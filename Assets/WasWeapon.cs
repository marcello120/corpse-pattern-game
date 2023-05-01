using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasWeapon : Weapon
{
    private Rigidbody2D rb;
    private Animator animator;

    public float weaponAttackPower = 10;
    public float weaponKnockback = 10;

    public float speed = 10;
    public bool attackQueued = false;
    public float timeSinceAttack = 0;
    public bool canAttack = true;


    private bool attackState = false;

    public WeaponSwing swing;

    public float reach;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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



    public override void Attack()
    {
        animator.SetTrigger("Attack");


        if (canAttack)
        {
            Transform playerTransform = transform;
            float offset = reach;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 attackDirection = (mousePosition - (Vector2)playerTransform.position).normalized;
            Vector2 attackPosition = (Vector2)playerTransform.position + attackDirection * offset;

            // Calculate rotation to face the mouse position
            Vector2 direction = mousePosition - (Vector2)playerTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            swing.InitWeaponAttack(weaponKnockback, weaponAttackPower);

            Instantiate(swing, attackPosition, rotation);
            
            
            attackQueued = false;
            canAttack = false;
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



  


}

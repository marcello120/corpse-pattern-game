using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasWeapon : Weapon
{
    private Rigidbody2D rb;
    private Animator animator;

    public float weaponAttackPower = 10;
    public float weaponKnockback = 10;
    public float baseWeaponReach = 3.35f;
    public float currentWeaponReach = 0f;

    public float speed = 10;
    public bool attackQueued = false;
    public float timeSinceAttack = 0;
    public bool canAttack = true;


    private bool attackState = false;

    public WeaponSwing swing;

    public float reach;
    public float size = 1;

    private Vector3 baseSize;


    public GameObject actualWas;


    // Start is called before the first frame update
    void Start()
    {
        rb = actualWas.GetComponent<Rigidbody2D>();
        animator = actualWas.GetComponent<Animator>();
        currentWeaponReach = baseWeaponReach;
        baseSize = transform.localScale;
        increaseReach(0f);
        increaseSize(0f);


        //InvokeRepeating(nameof(Attack), 0f, 0.5f); // Update the path every 0.5 seconds

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


    public override void increaseReach(float reachIncreae)
    {
        currentWeaponReach += reachIncreae;
        transform.localPosition = new Vector3(currentWeaponReach, transform.localPosition.y, actualWas.transform.localPosition.z);
    }

    public override void increaseSize(float sizeIncrease)
    {
        size *= (1 + sizeIncrease);
        transform.localScale = baseSize * size;
    }




    public override void Attack()
    {
        animator.SetTrigger("Attack");


        if (canAttack)
        {
            Transform playerTransform = actualWas.transform;
            float offset = reach;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 attackDirection = (mousePosition - (Vector2)playerTransform.position).normalized;
            Vector2 attackPosition = (Vector2)playerTransform.position + attackDirection * offset;

            // Calculate rotation to face the mouse position
            Vector2 direction = mousePosition - (Vector2)playerTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            swing.InitWeaponAttack(weaponKnockback, weaponAttackPower);

            WeaponSwing effect = Instantiate(swing, transform);
            effect.transform.localScale *= size;

            //comment if tired
           // effect.transform.SetParent(transform, true);


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasWeapon : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
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


    public override void Attack()
    {
        animator.SetTrigger("Attack");


        if (canAttack)
        {
            Transform playerTransform = weaponBody.transform;
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
            effect.transform.localScale *= size*10;

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

    public override void HeavyAttack()
    {
        animator.SetTrigger("Attack");


        if (canAttack)
        {
            Transform playerTransform = weaponBody.transform;
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
            effect.transform.localScale *= size * 25;

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

}

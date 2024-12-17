using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearWeapon :Weapon
{


// Start is called before the first frame update
void Start()
{
    Init();
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
        }
    }
}

    public override void Attack()
    {
        if (canAttack)
        {
            animator.SetTrigger("Attack");


            WeaponSwing effect = Instantiate(swing, transform);
            effect.InitWeaponAttack(weaponKnockback, weaponAttackPower);
            effect.transform.localScale *= size * 0.7f;
            effect.transform.localScale = new Vector3(effect.transform.localScale.x, effect.transform.localScale.y);

            canAttack = false;

        }

    }

    public override void HeavyAttack()
    {
        if (canAttack)
        {
            animator.SetTrigger("HeavyAttack");


            WeaponSwing effect = Instantiate(swing, transform);
            effect.InitWeaponAttack(weaponKnockback, weaponAttackPower, statusEffect);
            effect.transform.localPosition += new Vector3(0.5f, 0f);
            effect.transform.localScale *= size * 0.7f;

            canAttack = false;
        }
    }

}

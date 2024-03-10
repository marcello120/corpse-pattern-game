using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScimitarWeapon : Weapon
{
    public int combostep = 0;

    public WeaponSwing swing1;

    public MuliTimer comboTimer = new MuliTimer(1f);


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    void FixedUpdate()
    {
        if (!comboTimer.isDone() && combostep ==1)
        {
            comboTimer.update(Time.deltaTime);
        }
        else
        {
            animator.SetTrigger("Reset");
            combostep = 0;
            comboTimer.reset();
        }

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

            if (combostep == 0)
            {
                comboTimer.reset();
                WeaponSwing effect = Instantiate(swing, transform);
                effect.InitWeaponAttack(weaponKnockback, weaponAttackPower);
                effect.transform.localScale *= size * 0.7f;
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, effect.transform.localScale.y);

                combostep =1;
                return;
            }
            if(combostep == 1)
            {
                WeaponSwing effect = Instantiate(swing1, transform);
                effect.InitWeaponAttack(weaponKnockback, weaponAttackPower);
                effect.transform.localScale *= size;
                effect.transform.localScale = new Vector3(effect.transform.localScale.x, effect.transform.localScale.y);

                canAttack = false;
                combostep = 0;
                return;
            }

        }
    }
}

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

    public override void HeavyAttack()
    {
        if (canAttack)
        {
            CameraShake.Instance.Shake(0.33f, 0.3f);
            animator.SetTrigger("HeavyAttack");

            comboTimer.reset();
            WeaponSwing effect = Instantiate(swing, transform);
            effect.InitWeaponAttack(weaponKnockback, weaponAttackPower);
            effect.transform.localScale *= size * 1.5f;
            effect.transform.localPosition = new Vector3(effect.transform.localPosition.x, effect.transform.localPosition.y);
            effect.transform.localScale = new Vector3(effect.transform.localScale.x, effect.transform.localScale.y);

            comboTimer.reset();
            WeaponSwing effect2 = Instantiate(swing, transform);
            effect2.transform.localPosition = new Vector3(effect2.transform.localPosition.x -0.75f, effect2.transform.localPosition.y);
            effect2.InitWeaponAttack(weaponKnockback, weaponAttackPower);
            effect2.transform.localScale *= size * 1.5f;
            effect2.transform.localScale = new Vector3(effect.transform.localScale.x*-1f, effect.transform.localScale.y);

            combostep = 0;
            return;
        }
    }

    public override void Attack()
    {

        if (canAttack)
        {
            CameraShake.Instance.Shake(0.33f, 0.3f);
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

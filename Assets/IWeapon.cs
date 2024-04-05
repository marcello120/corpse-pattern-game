using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Attack();

    public void HeavyAttack();

    public void Charge(float amount);

    void increaseReach(float reachIncreae);
    public bool isAttacking();


}

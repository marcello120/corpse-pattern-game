using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Attack();

    public void HeavyAttack();

    void increaseReach(float reachIncreae);
    public bool isAttacking();


}

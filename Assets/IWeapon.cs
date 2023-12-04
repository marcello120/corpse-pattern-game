using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public void Attack();
    void increaseReach(float reachIncreae);
    public bool isAttacking();


}

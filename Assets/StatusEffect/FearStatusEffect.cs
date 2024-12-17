using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffects/Fear")]
public class FearStatusEffect : StatusEffect
{
    public float originalSpeed;

    public override void applyFirst(Enemy enemy)
    {
        if (enemy.movemetSpeed > 0)
        {
            originalSpeed = enemy.movemetSpeed;
            enemy.movemetSpeed = originalSpeed * -1;
        }
    }

    public override void applyRemove(Enemy enemy)
    {
        enemy.movemetSpeed = originalSpeed;
    }

    public override void applyUpdate(Enemy enemy)
    {

    }


}

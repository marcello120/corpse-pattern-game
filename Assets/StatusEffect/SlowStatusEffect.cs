using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "StatusEffects/Slow")]
public class SlowStatusEffect : StatusEffect
{
    public float slowAmount;

    public float originalSpeed;

    public override void applyFirst(Enemy enemy)
    {
        originalSpeed = enemy.movemetSpeed;
        enemy.movemetSpeed= originalSpeed * slowAmount;
    }

    public override void applyRemove(Enemy enemy)
    {
        enemy.movemetSpeed = originalSpeed;
    }

    public override void applyUpdate(Enemy enemy)
    {

    }

     
}

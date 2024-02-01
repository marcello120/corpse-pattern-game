using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RedSquare: Enemy
{
    public void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void getHit(float damage, Vector2 knockback)
    {
        if (isStunned())
        {
            base.getHit(damage, knockback);
        }
    }
}

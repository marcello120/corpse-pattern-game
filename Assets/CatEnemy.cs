using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEnemy : Enemy
{
    public MuliTimer attackPrepTime = new MuliTimer(1f);

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        commonUpdate();

        if(state== State.Moving)
        {
            if (isPlayerMovingTowardsMe(target) && movemetSpeed > 0)
            {
                movemetSpeed *= -1;
            }
            if (!isPlayerMovingTowardsMe(target) && movemetSpeed < 0)
            {
                movemetSpeed *= -1;
            }

            if (Vector3.Distance(transform.position, target.position) < 0.1)
            {
                setState(State.Attacking);
            }

        }
        if(state == State.Attacking)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.2)
            {
                setState(State.Moving);
            }

        }


    }

    public bool isPlayerMovingTowardsMe(Transform target)
    {
        if(target.GetComponent<Rigidbody2D>() == null)
        {
            return false;
        }

        // Calculate direction from myTransform to player
        Vector2 directionToPlayer = target.position - transform.position;

        // Calculate dot product of direction to player and player's velocity
        float dotProduct = Vector2.Dot(directionToPlayer.normalized, target.GetComponent<Rigidbody2D>().velocity.normalized);

        // If dot product is positive, player is moving towards myTransform
        // If dot product is negative, player is moving away from myTransform
        return dotProduct < 0f;
    }

}

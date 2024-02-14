using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class PredictiveAStarMovement : AStarMovement
{

    public float predictionFactor = 1f;


    void Start()
    {
        base.init();
    }

    public override void UpdatePath()
    {
        if (enemy.target == null)
        {
            return;
        }
        if (seeker.IsDone())
        {
            if (enemy.target.GetComponent<Rigidbody2D>() == null)
            {
                seeker.StartPath(transform.position, enemy.target.position, OnPathComplete);
            }
            else
            {
                seeker.StartPath(transform.position, PredictFuturePosition(), OnPathComplete);
            }
        }
    }

    Vector3 PredictFuturePosition()
    {
        if (enemy.target == null)
            return Vector3.zero;

        return enemy.target.position + (Vector3)enemy.target.GetComponent<Rigidbody2D>().velocity * predictionFactor;
    }


    private void FixedUpdate()
    {

        if (!preMoveChecksDone())
        {
            return;
        }

        // Move character towards next waypoint
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        enemy.moveInDirection(direction);

        // Check if close enough to the current waypoint, then proceed to the next one
        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDist)
        {
            currentWaypoint++;
        }
    }
}

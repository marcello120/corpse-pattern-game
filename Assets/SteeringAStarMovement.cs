using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SteeringAStarMovement : AStarMovement
{

    public float maxSpeed = 800f; // Maximum speed of movement
    public float slowingDistance = 5f; // Distance at which to start slowing down


    void Start()
    {
        init();
    }


    private void FixedUpdate()
    {
        if (!preMoveChecksDone())
        {
            return;
        }
        float gridsize = GameManager.Instance.gridCellSize;
        // Direction to the next waypoint
        Vector2 direction = ((Vector3)path.vectorPath[currentWaypoint] - enemy.transform.position).normalized;

        // Distance to the next waypoint
        float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);

        // Calculate the desired velocity based on the distance to the next waypoint
        float desiredSpeed = maxSpeed;
        if (distance < slowingDistance)
        {
            desiredSpeed = maxSpeed * (distance / slowingDistance);
        }

        enemy.moveInDirectionWithSpeedModifier(direction,desiredSpeed);


        // Check if close enough to the current waypoint, then proceed to the next one
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWayPointDist)
        {
            currentWaypoint++;
        }
    }
}

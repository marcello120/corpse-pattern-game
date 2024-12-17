using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectedSteeringAStarMovement : AStarMovement
{

    public float maxSpeed = 4f; // Maximum speed of movement
    public float slowingDistance = 4f; // Distance at which to start slowing down


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
        Vector3 newPos = (Vector3)path.vectorPath[currentWaypoint];
        if (Vector3.Distance(transform.position, enemy.target.position) < 1f)
        {
            float gridsize = GameManager.Instance.gridCellSize;
            newPos = Grid.adjustWoldPosToNearestCell((Vector3)path.vectorPath[currentWaypoint], gridsize);
        }

        Vector2 direction = (newPos - enemy.transform.position).normalized;

        // Distance to the next waypoint
        float distance = Vector2.Distance(enemy.transform.position, enemy.target.position);

        // Calculate the desired velocity based on the distance to the next waypoint
        float desiredSpeed = maxSpeed;
        if (distance < slowingDistance)
        {
            desiredSpeed = maxSpeed * (distance / slowingDistance);
        }

        enemy.moveInDirectionWithSpeedModifier(direction, desiredSpeed);


        // Check if close enough to the current waypoint, then proceed to the next one
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWayPointDist)
        {
            currentWaypoint++;
        }
    }
}


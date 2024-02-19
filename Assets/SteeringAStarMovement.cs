using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Enemy))]
public class SteeringAStarMovement : MonoBehaviour
{
    private Enemy enemy;

    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;

    public float maxSpeed = 800f; // Maximum speed of movement
    public float slowingDistance = 5f; // Distance at which to start slowing down

    Seeker seeker;

    public float nextWayPointDist = 0.2f;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        seeker = GetComponent<Seeker>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);

    }


    private void UpdatePath()
    {
        if (enemy.target == null)
        {
            return;
        }
        if (seeker.IsDone())
            seeker.StartPath(transform.position, enemy.target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    private void FixedUpdate()
    {
        if (path == null || (enemy.state != Enemy.State.Moving && enemy.state != Enemy.State.Idle))
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

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

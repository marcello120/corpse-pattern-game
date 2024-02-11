using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StrafingAStarMovement : MonoBehaviour
{
    public float circleRadius = 5.0f;
    public float collisionThreshold = 1f;
    public float nextWayPointDist = 0.2f;

    private Enemy enemy;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;

    public bool chasing = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemy = GetComponent<Enemy>();
        InvokeRepeating("UpdatePath", 0f, 0.5f); // Update path every 0.5 seconds
        if (enemy.target == null)
        {
            enemy.target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void UpdatePath()
    {

        if (seeker.IsDone())
        { 
            if(chasing)
            {
                seeker.StartPath(transform.position, enemy.target.position, OnPathComplete);
            }
            else
            {
                // Calculate the direction vector from the enemy to the player
                Vector3 toPlayer = enemy.target.position - transform.position;

                // Calculate the perpendicular vector for circle strafing
                Vector3 circleDir = new Vector3(-toPlayer.y, toPlayer.x, 0).normalized;

                // Calculate the target position on the circle
                Vector3 targetPosition = enemy.target.position + circleDir * circleRadius;

                // Move towards the target position using A* pathfinding
                seeker.StartPath(transform.position, targetPosition, OnPathComplete);
            }
        }

    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if(enemy.state != Enemy.State.Moving)
        {
            return;
        }

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Direction to the next waypoint
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        // Move the enemy
        enemy.moveInDirection(direction);

        // Check if the enemy is close enough to the current waypoint to switch to the next waypoint
        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDist)
        {
            currentWaypoint++;
        }

        // Check for collision with the player

        if (enemy.target == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, enemy.target.position);

        if (distanceToPlayer < collisionThreshold && !chasing)
        {
            chasing = true;
            
        }
        if(distanceToPlayer > collisionThreshold && chasing)
        {
            chasing = false;
        }
    }
}

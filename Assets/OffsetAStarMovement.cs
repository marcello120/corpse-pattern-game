using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OffsetAStarMovement : MonoBehaviour
{
    private Seeker seeker;
    private Enemy enemy;
    private Path path;
    private int currentWaypoint = 0;

    public float nextWaypointDistance = 0.2f;
    public float pathOffset = 1f;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        enemy = GetComponent<Enemy>();


        if (enemy.target == null)
        {
            enemy.target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Start calculating a path to the targetPosition
        InvokeRepeating(nameof(UpdatePath), 0f, 2f);
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

            // Apply path offset
            for (int i = 0; i < path.vectorPath.Count; i++)
            {
                path.vectorPath[i] += GetPathOffset(i);
            }

            currentWaypoint = 0;
        }
    }

    private Vector3 GetPathOffset(int index)
    {
        // Calculate a simple offset based on the index of the waypoint
        // You can customize this function for more complex offset logic
        Vector3 offset = Vector3.zero;
        offset.x = Mathf.Sin(index * 0.5f) * pathOffset;
        offset.y = Mathf.Cos(index * 0.5f) * pathOffset;
        return offset;
    }

    private void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Move towards the current waypoint
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        enemy.moveInDirection(direction);

        // Check if the character is close enough to the current waypoint to move to the next one
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}

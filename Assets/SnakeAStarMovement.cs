using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SnakeAStarMovement : AStarMovement
{
    [Header("Snake Movement Settings")]
    public float curveFrequency = 2f; // Frequency of the curve
    public float curveMagnitude = 2f; // Magnitude of the curve
    public float variance = 0.5f;

    void Start()
    {
        init();
        curveFrequency = curveFrequency + UnityEngine.Random.Range(-variance, variance);
        curveMagnitude = curveMagnitude + UnityEngine.Random.Range(-variance, variance);

    }

    private void FixedUpdate()
    {
        if (!preMoveChecksDone())
        {
            return;
        }

        // Move character towards next waypoint with snake-like curved movement
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 snakeDirection = SnakeCurve(direction);
        enemy.moveInDirection(snakeDirection);

        // Check if close enough to the current waypoint, then proceed to the next one
        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDist)
        {
            currentWaypoint++;
        }
    }

    // Introduce a curve to the movement direction
    private Vector2 SnakeCurve(Vector2 direction)
    {
        float time = Time.time * curveFrequency;
        float curveX = Mathf.Sin(time) * curveMagnitude;
        float curveY = Mathf.Cos(time) * curveMagnitude; // Changed Z-axis (3D) to Y-axis (2D)
        Vector2 curve = new Vector2(curveX, curveY);
        return direction + curve;
    }
}
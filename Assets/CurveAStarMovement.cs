using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[RequireComponent(typeof(Enemy))]
public class CurveAStarMovement : MonoBehaviour
{
    private Seeker seeker;
    private Enemy enemy;
    private Path path;
    private int currentWaypoint = 0;

    public float nextWaypointDistance = 0.2f;
    public float curveRadius = 1f;
    public float amplitude = 1f; // Amplitude of the curve
    public float frequency = 1f; // Frequency of the curve
    public int segments = 10;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        enemy = GetComponent<Enemy>();
        if (enemy.target == null)
        {
            enemy.target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Start calculating a path to the targetPosition
        InvokeRepeating(nameof(UpdatePath), 0f, 2f); // Update the path every 0.5 seconds
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

            // Apply curve to the path
            Vector3[] originalPath = path.vectorPath.ToArray();
            path.vectorPath.Clear();

            for (int i = 0; i < originalPath.Length - 1; i++)
            {
                Vector3 startPoint = originalPath[i];
                Vector3 endPoint = originalPath[i + 1];

                // Use Bezier curve interpolation to generate curved waypoints
                Vector3[] curvePoints = BezierCurve(startPoint, endPoint, curveRadius, amplitude, frequency, segments);

                foreach (Vector3 curvePoint in curvePoints)
                {
                    path.vectorPath.Add(curvePoint);
                }
            }

            currentWaypoint = 0;
        }
    }

    private void FixedUpdate()
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

    // Bezier Curve Interpolation with amplitude and frequency
    private Vector3[] BezierCurve(Vector3 start, Vector3 end, float radius, float amplitude, float frequency, int segments)
    {
        Vector3[] curvePoints = new Vector3[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            float x = Mathf.Lerp(start.x, end.x, t);
            float y = Mathf.Lerp(start.y, end.y, t);

            // Apply the curve to the y-coordinate with amplitude and frequency
            float curveOffset = Mathf.Sin(t * Mathf.PI * 2 * frequency) * amplitude;
            y += curveOffset;

            curvePoints[i] = new Vector3(x, y, 0);
        }

        return curvePoints;
    }
}

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class ComplexAStarMovement : MonoBehaviour
{
    public enum Mode
    {
        TRANSFORM,
        POSITION
    }

    [Header("Debug")]

    public Enemy enemy;

    public Vector3 position;

    public Mode mode;

    public Path path;
    public int currentWaypoint;
    public bool reachedEndOfPath;

    public Seeker seeker;

    [Header("Set these")]

    public float nextWayPointDist = 0.2f;

    public float refreshTime = 0.5f;


    public void init()
    {
        enemy = GetComponent<Enemy>();

        seeker = GetComponent<Seeker>();

        InvokeRepeating(nameof(UpdatePath), 0f, refreshTime);
    }


    public virtual void UpdatePath()
    {
        if (enemy.target == null)
        {
            return;
        }
        if (seeker.IsDone())
            seeker.StartPath(transform.position, enemy.target.position, OnPathComplete);
    }

    public virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public virtual bool preMoveChecksDone()
    {
        if (enemy.target == null)
        {
            return false;
        }
        if (path == null || !enemy.moveStates.Contains(enemy.state))
        {
            return false;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return false;
        }
        else
        {
            reachedEndOfPath = false;
        }

        return true;
    }

}

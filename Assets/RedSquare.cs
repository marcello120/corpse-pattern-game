using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RedSquare : Enemy
{
    public GameObject body;
    public Transform target;

    Path path;
    int currentWaypoint;
    bool reachedEndOfPath;

    Seeker seeker;

    public float nextWayPointDist = 1f;
    void Start()
    {
        base.Init();
        spriteRenderer = body.GetComponent<SpriteRenderer>();
        animator = body.GetComponent<Animator>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        seeker = GetComponent<Seeker>();


        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f); // Update the path every 0.5 seconds


        //seeker.StartPath(rb.position,target.position, onPathComplete);


    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void Death()
    {
        rb.velocity= Vector3.zero;
        base.Death();
    }

    private void FixedUpdate()
    {
        if (path == null || isDead)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        // Move character towards next waypoint
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 force = direction * movemetSpeed * Time.fixedDeltaTime;

        rb.AddForce(force);

        // Check if close enough to the current waypoint, then proceed to the next one
        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDist)
        {
            currentWaypoint++;
        }
    }
}
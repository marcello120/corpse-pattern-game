using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Scarab : Enemy
{
    public Transform target;

    public float gridsize = 0.5f;

    public State state = State.Waiting;

    public float waitTime = 1f;

    public float waitCounter = 0f;

    public Vector3 destinationCell;

    public enum State
    {
        Idle,
        Waiting,
        Moving
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if waiting, correct self to grid
        if(state == State.Waiting)
        {
            waitCounter += Time.deltaTime;
            if(waitCounter > waitTime)
            {
                waitCounter= 0f;
                state = State.Moving;

            }

            Vector3 adjustedpos = adjustWoldPosToNearestCell(transform.position);
            if (Vector3.Distance(transform.position, adjustedpos) > 0.01f)
            {
                transform.position = adjustedpos;

            }

        }
        

        else if(state == State.Moving)
        {
           if(destinationCell == null || destinationCell == Vector3.zero)
            {
                setDestination(target.transform.position);

            }
            else if(health > 0)
            {

                Vector3 directionToTarget = (destinationCell - transform.position).normalized;


                rb.AddForce(directionToTarget * movemetSpeed);

                Debug.DrawLine(transform.position, transform.position + directionToTarget);

                if(Vector3.Distance(transform.position, destinationCell) < 0.1f)
                {
                    destinationCell = Vector3.zero;
                    state = State.Waiting;
                }

            }
        }

    }

    private void setDestination(Vector3 initialTarget)
    {

        List<Vector3> neighbors = getNeighbors();

        Vector3 closest = neighbors.ToArray()[0];
        float mindist = 10000;

        foreach (Vector3 neighbor in neighbors)
        {
            float dist = Vector3.Distance(initialTarget, neighbor);
            if (dist < mindist)
            {
                mindist = dist;
                closest = neighbor;
            }
        }

        destinationCell = closest;
    }

    private void bounce(Vector3 initialTarget)
    {

        List<Vector3> neighbors = getNeighbors();

        Vector3 closest = neighbors.ToArray()[0];
        float maxdist = 0;

        foreach (Vector3 neighbor in neighbors)
        {
            float dist = Vector3.Distance(initialTarget, neighbor);
            if (dist > maxdist)
            {
                maxdist = dist;
                closest = neighbor;
            }
        }

        destinationCell = closest;
    }

    private List<Vector3> getNeighbors()
    {
        List<Vector3> neighbors = new List<Vector3>();
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize*-1,gridsize*-1)));
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * -1)));
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 0, gridsize * -1)));

        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * -1, gridsize * 1)));
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * 1)));
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 0, gridsize * 1)));

        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * -1, gridsize * 0)));
        neighbors.Add(adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * 0)));

        return neighbors;
    }
 

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
        if (collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "Wall")
        {

            //collision.AddForce(direction * -2000f);
            bounce(collision.transform.position);
        }
    }


    public Vector3 adjustWoldPosToNearestCell(Vector3 worldPos)
    {
        Vector3 pos = worldPos;
        float nearestX = findMultiple(pos.x, gridsize);
        float nearestY = findMultiple(pos.y, gridsize);
        Vector3 aprox = new Vector3(nearestX, nearestY, 0);

        float xDiff = aprox.x - pos.x;
        float yDiff = aprox.y - pos.y;

        if (xDiff < 0)
        {
            nearestX += gridsize / 2;
        }
        else
        {
            nearestX -= gridsize / 2;
        }

        if (yDiff < 0)
        {
            nearestY += gridsize / 2;
        }
        else
        {
            nearestY -= gridsize / 2;
        }

        Vector3 finalPos = new Vector3(nearestX, nearestY);

        return finalPos;
    }

    private float findMultiple(float value, float factor)
    {
        float nearestMultiple =
                (float)Math.Round(
                     (value / (float)factor),
                     MidpointRounding.AwayFromZero
                 ) * factor;

        return nearestMultiple;
    }


}

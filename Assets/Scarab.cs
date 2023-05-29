using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool gofirst;

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
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
                animator.SetBool("IsMoving",true);

            }

            Vector3 adjustedpos = Grid.adjustWoldPosToNearestCell(transform.position,gridsize);
            if (Vector3.Distance(transform.position, adjustedpos) > 0.1f)
            {
                Vector3 directionToTarget = (adjustedpos - transform.position).normalized;
                rb.AddForce(directionToTarget * movemetSpeed);


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
                    animator.SetBool("IsMoving", false);
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

    public void bounce(Vector3 initialTarget)
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
        List<Vector3> neighbors = new List<Vector3>
        {
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * -1, gridsize * -1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * -1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 0, gridsize * -1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * -1, gridsize * 1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * 1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 0, gridsize * 1), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * -1, gridsize * 0), gridsize),
            Grid.adjustWoldPosToNearestCell(transform.position + new Vector3(gridsize * 1, gridsize * 0), gridsize)
        };

        return neighbors;
    }

    public override void Death()
    {
        base.Death();
       Collider2D[] ccs = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D cc in ccs)
        {
            cc.enabled= false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gofirst = false;
        if (collision.gameObject.GetComponent<Scarab>() !=null) 
        {
            Scarab other = collision.gameObject.GetComponent<Scarab>();
            if (other.gofirst)
            {
                state = State.Waiting;
            }
            else
            {
                setDestination(target.transform.position);
                gofirst = true;
            }

        }
        else if ( collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy")
        {
           bounce(collision.transform.position);
        }

    }

}

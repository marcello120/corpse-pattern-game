using Pathfinding;
using System.Collections.Generic;
using UnityEngine;


public class Scarab : Enemy
{
    public float waitTime = 1f;

    public float waitCounter = 0f;

    public Vector3 destinationCell;


    public GameObject deathMarker;

    public GameObject stunMarker;

    private GameObject StunMarkerInstance;

    public GameObject hitEffect;

    private float moveTimer;
    private float moveTimerLimit = 1f;


    Seeker seeker;

    Path path;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        seeker = GetComponent<Seeker>();
 
        
    }

    private void UpdatePath()
    {
        if (target == null)
        {
            return;
        }
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
        }
        if(path.vectorPath.Count < 4)
        {
            setDestination(target.position);
        }
        else
        {
            setDestination(path.vectorPath[3]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        if (isStunned())
        {
            // stunTimer += Time.deltaTime;
            //if (stunTimer > stunTime)
            //{
            //    stunTimer = 0;
            //    stunned = false;
            //    removeStunMarker();
            //}
            return;
        }

        //if waiting, correct self to grid
        if (state == State.Waiting)
        {
            moveTimer = 0;
            waitCounter += Time.deltaTime;
            if (waitCounter > waitTime)
            {
                waitCounter = 0f;
                UpdatePath();
                state = State.Moving;
                animator.SetBool("IsMoving", true);


            }

            Vector3 adjustedpos = Grid.adjustWoldPosToNearestCell(transform.position, GameManager.Instance.gridCellSize);
            if (Vector3.Distance(transform.position, adjustedpos) > 0.1f)
            {
                Vector3 directionToTarget = (adjustedpos - transform.position).normalized;
                rb.AddForce(directionToTarget * movemetSpeed);


            }

        }
        else if (state == State.Moving)
        {

            if (moveTimer > moveTimerLimit)
            {
                UpdatePath();
                moveTimer = 0;
            }
            else
            {
                moveTimer += Time.deltaTime;
            }

           if(destinationCell == null || destinationCell == Vector3.zero)
            {
                if (path == null || path.vectorPath.Count < 4)
                {
                    setDestination(target.position);
                }
                else
                {
                    setDestination(path.vectorPath[3]);
                }

            }
            else if(health > 0)
            {

                Vector3 directionToTarget = (destinationCell - transform.position).normalized;


                rb.AddForce(directionToTarget * movemetSpeed);

                //Debug.DrawLine(transform.position, transform.position + directionToTarget);

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
        //Debug.DrawLine(transform.position, initialTarget,Color.magenta,500f);
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
        float gridsize = GameManager.Instance.gridCellSize;
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


    public override void getHit(float damage, Vector2 knockback)
    {
        base.getHit(damage, knockback);
        //check if next attack kills
        Instantiate(hitEffect,transform.position,Quaternion.identity);

    }

    private void removeStunMarker()
    {
        Destroy(StunMarkerInstance);
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
        if (collision.gameObject.GetComponent<Scarab>() !=null) 
        {
            float mydist = Vector3.Distance(transform.position, target.position);
            float otherdist =  Vector3.Distance(collision.transform.position, target.position);
            if (otherdist < mydist)
            {
                state = State.Waiting;
            }
            else
            {
                if (path == null || path.vectorPath.Count < 4)
                {
                    setDestination(target.position);
                }
                else
                {
                    setDestination(path.vectorPath[3]);
                }
            }

        }
        else if ( collision.gameObject.tag == "Wall")
        {
           //bounce(collision.transform.position);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    public DetectionZoneController detectionZoneController;
    public GameObject tounge;

    public enum State
    {
        Attacking,
        Moving,
        Idle
    }


    public GameObject player;

    public float chaseDistance;
    public float attackDistance;

    public float attackDelay;
    public float attackTime;

    public State state;

    private Animator toungeAnimtor;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        player = GameObject.FindGameObjectWithTag("Player");
        state = State.Idle;
        toungeAnimtor= tounge.GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //moveToPlayerWithDetectionZone(detectionZoneController);

        if (player == null)
        {
            return;
        }

        isMoving = false;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= chaseDistance)
        {
            Vector2 directionToTarget = (player.transform.position - transform.position).normalized;

            handleFlip(-1,directionToTarget);

            if (distance < attackDistance)
            {
                //stop and perform attack
                if (attackTime >= attackDelay)
                {
                    //perform attack
                    attackTime = 0;
                    Debug.Log("ATTACK!");
                    toungeAnimtor.SetTrigger("Attack");
                    
                }
            }
            else
            {
                //move to player
                rb.AddForce(directionToTarget * movemetSpeed);
                isMoving = true;
            }
        }

        if (attackTime < attackDelay)
        {
            attackTime += Time.deltaTime;
        }

        animator.SetBool("IsMoving", isMoving);



    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
    }
}


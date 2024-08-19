using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CatEnemy : Enemy
{
    public MuliTimer attackPrepTimer = new MuliTimer(1f);
    public MuliTimer postAttackTimer = new MuliTimer(1f);

    private PolygonCollider2D colli; 


    public GameObject clawEffect;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        colli = GetComponentInChildren<PolygonCollider2D>();
    }

    private void invertSpeed(float distToPlayer)
    {
        if (isPlayerMovingTowardsMe(target) && movemetSpeed > 0 && distToPlayer < 10f)
        {
            movemetSpeed *= -0.75f;
        }
        if (!isPlayerMovingTowardsMe(target) && movemetSpeed < 0)
        {
            movemetSpeed *= -1.33333333f;
        }
    }

    private void Update()
    {
        commonUpdate();
        if(target == null)
        {
            return;
        }

        if(state== State.Moving)
        {
            float distToPlayer = Vector3.Distance(transform.position, target.position);

            invertSpeed(distToPlayer);
            if (distToPlayer < 0.4)
            {
                setState(State.Attacking);
            }

        }
        if(state == State.Attacking)
        {
            if (!attackPrepTimer.isDone())
            {
                attackPrepTimer.update(Time.deltaTime);
            }
            else
            {
                StartCoroutine(attack());
                attackPrepTimer.reset();
                setState(State.Preparing);

            }
            if (Vector3.Distance(transform.position, target.position) > 0.6)
            {
                attackPrepTimer.reset();
                setState(State.Moving);
            }
        }
        if (state == State.Preparing)
        {

            if (!postAttackTimer.isDone())
            {
                postAttackTimer.update(Time.deltaTime);
            }
            else
            {
                postAttackTimer.reset();
                setState(State.Moving);
            }
            if (Vector3.Distance(transform.position, target.position) > 0.6)
            {
                attackPrepTimer.reset();
                setState(State.Moving);
            }
        }

    }
    public override void Death()
    {
        if ( movemetSpeed < 0)
        {
            movemetSpeed *= -1;
        }
        base.Death();
    }

    IEnumerator attack()
    {
        Vector3 direction = (transform.position -target.position).normalized;
        colli.transform.rotation = Quaternion.FromToRotation(Vector3.right, -direction);
        colli.enabled = true;
        Instantiate(clawEffect, transform.position + direction*0.1f, Quaternion.FromToRotation(Vector3.right, direction));
        yield return new WaitForSeconds(0.5f);
        colli.transform.rotation = Quaternion.identity;
        colli.enabled = false;
        yield return new WaitForSeconds(0.5f);
    }

    public bool isPlayerMovingTowardsMe(Transform target)
    {
        if(target.GetComponent<Rigidbody2D>() == null)
        {
            return false;
        }

        // Calculate direction from myTransform to player
        Vector2 directionToPlayer = target.position - transform.position;

        // Calculate dot product of direction to player and player's velocity
        float dotProduct = Vector2.Dot(directionToPlayer.normalized, target.GetComponent<Rigidbody2D>().velocity.normalized);

        // If dot product is positive, player is moving towards myTransform
        // If dot product is negative, player is moving away from myTransform
        return dotProduct < 0f;
    }

}

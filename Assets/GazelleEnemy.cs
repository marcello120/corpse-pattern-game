using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazelleEnemy : Enemy
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


    private void Update()
    {
        commonUpdate();
        if (target == null)
        {
            return;
        }

        if (state == State.Moving)
        {
            float distToPlayer = Vector3.Distance(transform.position, target.position);

            if (distToPlayer < 0.5)
            {
                setState(State.Attacking);
            }

        }
        if (state == State.Attacking)
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
            if (Vector3.Distance(transform.position, target.position) > 0.7)
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

    IEnumerator attack()
    {
        Vector3 direction = (transform.position - target.position).normalized;
        colli.transform.rotation = Quaternion.FromToRotation(Vector3.right, -direction);
        colli.enabled = true;
        Instantiate(clawEffect, transform.position + direction * 0.1f, Quaternion.FromToRotation(Vector3.right, direction));
        yield return new WaitForSeconds(0.5f);
        colli.transform.rotation = Quaternion.identity;
        colli.enabled = false;
        yield return new WaitForSeconds(0.5f);
    }
}

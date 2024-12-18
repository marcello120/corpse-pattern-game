using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplodeEnemy : Enemy
{
    public Animator effects;

    public MuliTimer fuseTimer= new MuliTimer(3f);

    public float expRadius = 0.6f;

    public float boomDist = 0.2f;

    public float knockbackPower = 1;

    public AudioClip ticktock;
    public AudioClip boom;

    // Start is called before the first frame update
    public void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void setState(State stateIn)
    {
        if(stateIn == State.Attacking && state == State.Moving)
        {
            rb.mass = 999999999;
        }
        if(state == State.Attacking)
        {
            rb.mass = 1;

        }
        base.setState(stateIn);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, boomDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, expRadius);

    }

    // Update is called once per frame
    void Update()
    {
       
        if(target == null)
        {
            return;
        }

        if (state== State.Moving)
        {
            if (Vector3.Distance(transform.position, target.position) < boomDist)
            {
                float gridsize = GameManager.Instance.gridCellSize;
                Vector3 gridCenter = Grid.adjustWoldPosToNearestCell(transform.position, gridsize);
                if(Vector3.Distance(transform.position, gridCenter) < 0.1)
                {
                    effects.SetBool("Prep", true);
                    knockbackReduction = 1;
                    setState(State.Attacking);
                    audioSource.Stop();
                    audioSource.PlayOneShot(ticktock);
                }
            }
            return;
        }
        if(state == State.Attacking)
        {
            if (isStunned())
            {
                fuseTimer.reset();
                effects.SetBool("Prep", false);
                audioSource.Stop();
                if (health > 0)
                {
                    setState(State.Moving);
                    knockbackReduction = 0.5f;
                }
                return;
            }

            if (!fuseTimer.isDone())
            {
                fuseTimer.update(Time.deltaTime);
            }
            else
            {
                fuseTimer.reset();
                effects.SetBool("Prep", false);
                effects.SetTrigger("Exp");
                StartCoroutine(Explode());
                if (health > 0)
                {
                    setState(State.Base);
                    knockbackReduction = 0.5f;
                }
            }
            return;
        }
    }


    IEnumerator Explode()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(boom,5f);
        this.getHit(1f, Vector2.zero, Vector2.zero);


        yield return new WaitForSeconds(0.2f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, expRadius);
        //foreach collider in hitColliders
        foreach (Collider2D coll in hitColliders)
        {
            //enemy
            //if (coll.gameObject.GetComponent<Enemy>() != null)
            //{
            //    Vector2 direction = transform.position - coll.gameObject.transform.position;

            //    Vector2 knockback =  (direction.normalized * knockbackPower * -1);

            //    coll.gameObject.GetComponent<Enemy>().getHit(attackPower, knockback,direction);
            //}
            if (coll.gameObject.GetComponent<CorpseScript>() != null)
            {
                Debug.Log("CORPSE AT" + coll.transform.position);
            }
            //player
            if (coll.gameObject.GetComponent<RiggedPlayerController>() != null)
            {
                coll.gameObject.GetComponent<RiggedPlayerController>().takeDamage(attackPower, this.gameObject);
            }
        }
        yield return new WaitForSeconds(1.5f);
        setState(State.Moving);
        knockbackReduction = 0.5f;
    }
}

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Necrophage : Enemy
{

    public float corpseConsumetTime=15f;
    public float consumeTimer = 0f;

    public float chaseTime = 15f;
    public float chaseTimer = 0f;

    public bool poweredUp = false;

    public float baseMass = 1f;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //if idle
        //if no corpse --> find nearest corpse
        //if no corpse --> moving
        //if corpse --> go to corpse
        //if corpse and AT coprse ->eat corpse
        //if eating coprse --> set timer
        //if eating corpse and time is up --> corpse is eaten --> buff --> moving
        //if hit or player is within X radius --> moving 
        //if moving and timer is up --> idle

        if(poweredUp)
        {
            if(target ==null || target.tag!= "Player")
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;

            }
            //move to player;
            return;
        }


        if (state == State.Idle)
        {
            if (target == null || target.tag != "Corpse")
            {
                GameObject nearestCorpse = findNearestCorpse();
                if (nearestCorpse != null)
                {
                    target = nearestCorpse.transform;
                }
                else
                {
                    state = State.Moving;
                    return;
                }
            }

            if (target.tag == "Corpse")
            {
                if (atCorpse())
                {
                    state = State.Preparing;
                    rb.mass = 500;
                }
                else
                {
                    //move to corpse
                }
            }

        }
        else if (state == State.Moving)
        {
            if (target == null || target.tag == "Corpse")
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }

            if (chaseTimer < chaseTime)
            {
                chaseTimer += Time.deltaTime;
                //move to player
            }
            else
            {
                chaseTimer = 0f;
                state= State.Idle;
            }
        }
        else if(state == State.Preparing)
        {
            if (target == null)
            {
                state= State.Idle;
                rb.mass = baseMass;
                return;
            }

            if(consumeTimer < corpseConsumetTime)
            {
                consumeTimer += Time.deltaTime;
            }
            else
            {
                state= State.Moving;
                consumeTimer = 0f;
                eatCorpse();
                rb.mass = baseMass;
            }
        }

    }

 
    public override void getHit(float damage, Vector2 knockback)
    {
        base.getHit(damage, knockback);
        state = State.Moving;
        rb.mass = baseMass;
        if(health == 1 && poweredUp)
        {
            powerDown();
        }
    }

    private void powerUp()
    {
        health += 2;
        attackPower += 1;
        movemetSpeed *= 1.5f;
        body.transform.localScale *= 1.2f;
        spriteRenderer.material.SetColor("_Color", Color.red);
        poweredUp = true;
        state = State.Moving;
        statusHolder.removeDeathMarker(this);
    }

    private void powerDown()
    {
        attackPower = 1;
        movemetSpeed *= 0.75f;
        body.transform.localScale *= 0.8f;
        spriteRenderer.material.SetColor("_Color", Color.white);
        poweredUp = false;
        state = State.Idle;
    }

    private void eatCorpse()
    {
        if (target != null & target.tag == "Corpse")
        {
            GameManager.Instance.removeCorpseAtWorldPos(target.position); 
        }
        powerUp();
    }

    private bool atCorpse()
    {
       return target!=null && target.tag == "Corpse" && Vector3.Distance(transform.position, target.position) < 0.5f;

    }

    private GameObject findNearestCorpse()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Corpse");

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    }
}

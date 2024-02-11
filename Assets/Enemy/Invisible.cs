using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : Enemy
{

    public float frenzyDistToPlayer = 2f;
    public float baseSpeed = 1f;
    public float frenzySpeed = 200f;

    [Header("Timer")]


    public MuliTimer chaseTimer = new MuliTimer(15f);


    public MuliTimer prepareTimer = new MuliTimer(3f);

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        setState(State.Idle);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        turnInvisible();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, frenzyDistToPlayer);

    }

    // Update is called once per frame
    void Update()
    {

        //MOVES SLOW&INVIS TOWARDS PLAYER
        //IF near player REVEAL+ CHANGE
        // other enemy killed in Radius also REVEAL+ CHANGE
        //IF CHARGING FOR X sec  & out of player radius and no hit - go back to invis
        //has to wait X sec between changes 
        //if killed nearby while charging -> CHarge longer (reset timer)
        //if HIT ->  goes invies and moves X dist away


        if (state == State.Idle)
        {
            if (Vector3.Distance(transform.position, target.position) < frenzyDistToPlayer)
            {
                setState(State.Moving);
                baseSpeed = movemetSpeed;
                movemetSpeed = frenzySpeed;
                turnVisible();
                return;
            }
            return;
        }

        if(state == State.Moving)
        {

            if (!chaseTimer.isDone())
            {
                chaseTimer.update(Time.deltaTime);
                //move to player
            }
            else
            {
                chaseTimer.reset();
                setState(State.Preparing);
            }

            if (Vector3.Distance(transform.position, target.position) < 0.35f) //MULI MEG FOGJA OLDANI HOGY NE LEHESSEN KIHTALNI
            {
                movemetSpeed = baseSpeed;
                setState(State.Preparing);
            }
            return;
        }

        if(state == State.Preparing)
        {
            if (!prepareTimer.isDone())
            {
                prepareTimer.update(Time.deltaTime);
            }
            else
            {
                prepareTimer.reset();
                setState(State.Idle);
            }
        }
    }

    private void turnInvisible()
    {

    }

    private void turnVisible()
    {

    }
}

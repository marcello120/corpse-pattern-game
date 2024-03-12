using Pathfinding;
using System.Collections.Generic;
using UnityEngine;


public class Scarab : Enemy
{
    public Vector3 restingPlace;

    void Start()
    {
        base.Init();
        setState(State.Moving);
    }

    private void Update()
    {
        if(state== State.Dying) {
            if(Vector3.Distance(restingPlace,transform.position) >0.1)
            {
                Vector3 dir = (restingPlace - transform.position).normalized;
                moveInDirectionWithSpeedModifier(dir, 0.5f);
            }
        }
    }

    public override void Death()
    {

        rb.velocity = Vector3.zero;

        GameManager.CoprseInfoObject cio = GameManager.Instance.AddWorldPosToGridAndReturnAdjustedPos(transform.position, corpseNumber, powerLevel);

        Vector3 place = cio.corpseWorldPos;


        Collider2D[] ccs = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D cc in ccs)
        {
            cc.enabled = false;
        }

        if (corpse != null)
        {
            int corpseNum = cio.coprseNumber;

            GameObject newCorpse = Instantiate(corpse, place, Quaternion.identity);
            newCorpse.GetComponent<SpriteRenderer>().sprite = PatternStore.Instance.configs[corpseNum];
            newCorpse.GetComponent<CorpseScript>().Init(corpseNum);
            restingPlace = place;
        }
        else
        {
            Debug.LogError("Corspe must be set");
        }
        isDead = true;
        setState(State.Dying);
    }

}

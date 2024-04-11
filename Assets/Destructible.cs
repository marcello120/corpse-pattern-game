using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Destructible : MonoBehaviour
{
    public float health;
    public GameObject hitEffect;
    public bool blocking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float hitDestructible(float damage)
    {
        health -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (health < 1)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            if (blocking)
            {
                var guo = new GraphUpdateObject(GetComponent<Collider2D>().bounds);
                guo.updatePhysics = true;
                AstarPath.active.UpdateGraphs(guo);
            }
            Destroy(gameObject);

        }
        return health;
    }

}

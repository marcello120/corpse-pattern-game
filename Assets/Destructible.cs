using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float health;
    public GameObject hitEffect;

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
            Destroy(gameObject);
        }
        return health;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBodyScript : MonoBehaviour
{
    public Scarab parentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        parentEnemy = GetComponentInParent<Scarab>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Remove()
    {
        parentEnemy.Remove();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        parentEnemy.damagePlayer(collision.collider);
    }
}

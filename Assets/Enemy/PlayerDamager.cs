using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public Enemy parentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        parentEnemy= GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       parentEnemy.damagePlayer(collision.collider);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{

    public float damage = 1f;

    public GameObject expodeEffect;

    public float expRadius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.tag=="Enemy" && collision.GetComponent<EnemyHitbox>()!=null)
        {
            Instantiate(expodeEffect, transform.position, Quaternion.identity);
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, expRadius);
            //foreach collider in hitColliders
            foreach (Collider2D coll in hitColliders)
            {
                //enemy
                if (coll.GetComponent<EnemyHitbox>() != null)
                {
                    Vector3 directionToEnemy = (collision.gameObject.transform.position - transform.position).normalized;
                    collision.GetComponent<EnemyHitbox>().getHit(damage, directionToEnemy, directionToEnemy);
                }
            }
            Destroy(gameObject);
        }
    }
}

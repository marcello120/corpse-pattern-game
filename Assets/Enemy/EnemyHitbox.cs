using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox: MonoBehaviour
{
    public Enemy parentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Remove()
    {
        parentEnemy.Remove();
    }

}

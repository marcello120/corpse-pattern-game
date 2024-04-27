using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox: MonoBehaviour
{
    public Enemy parentEnemy;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public virtual void Init()
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

    public void RemoveAllStatuses()
    {
        parentEnemy.removeAllStatuses();
    }

    public virtual void getHit(float damage, Vector2 knockbac, Vector3 direction)
    {
        if (parentEnemy == null) return;
        parentEnemy.getHit(damage, knockbac, direction);
    }


}

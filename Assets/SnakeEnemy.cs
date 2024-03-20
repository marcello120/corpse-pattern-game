using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        float random = UnityEngine.Random.Range(0.75f, 1.5f);
        movemetSpeed *= random;
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        commonUpdate();

    }
}

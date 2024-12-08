using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        commonUpdate();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBoss : Enemy
{
    public void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        commonUpdate();

    }

}

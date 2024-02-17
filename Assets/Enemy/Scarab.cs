using Pathfinding;
using System.Collections.Generic;
using UnityEngine;


public class Scarab : Enemy
{

    void Start()
    {
        base.Init();
        setState(State.Moving);
    }

    private void Update()
    {
    }

}

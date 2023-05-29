using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : Enemy
{

    public DetectionZoneController detectionZoneController;

    // Start is called before the first frame update
    void Start()
    {
        enemyname = "Slime";
        base.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveToPlayerWithDetectionZone(detectionZoneController);

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
    }
}

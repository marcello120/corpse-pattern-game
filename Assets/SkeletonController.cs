using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : Enemy
{
    public DetectionZoneController DetectionZoneController;


    // Start is called before the first frame update
    void Start()
    {
        base.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveToPlayerWithDetectionZone(DetectionZoneController);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
    }
}

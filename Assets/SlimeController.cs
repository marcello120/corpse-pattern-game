using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : Enemy
{
    public float attackPower = 5;

    public float movemetSpeed = 10;

    public DetectionZoneController DetectionZoneController;

    // Start is called before the first frame update
    void Start()
    {
        health = 30;
        enemyname = "Slime";
        base.Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (DetectionZoneController.detectedObjs.Count > 0)
        {
            if (DetectionZoneController.detectedObjs[0] != null && health>0)
            {
                GameObject target = DetectionZoneController.detectedObjs[0];

                Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

                rb.AddForce(directionToTarget * movemetSpeed);
            }
        }  
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController= collision.gameObject.GetComponent<PlayerController>();
            playerController.takeDamage(attackPower,this);
            Debug.Log("TEST");
        }
    }
}

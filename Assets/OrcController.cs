using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OrcController : Enemy
{
    public float attackPower = 10;
    public float moveSpeed = 10;
    public DetectionZoneController DetectionZoneController;
    public float dirChangeChance = 1000;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        health = 60;
        enemyname = "Orc";
        base.Init();
        float random = Random.Range(0f, 260f);
        direction = new Vector2(Mathf.Sin(random), Mathf.Cos(random));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (DetectionZoneController.detectedObjs.Count > 0)
        {
            if (DetectionZoneController.detectedObjs[0] != null)
            {
                GameObject target = DetectionZoneController.detectedObjs[0];

                Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

                rb.AddForce(directionToTarget * moveSpeed);
            }
        }
        if(DetectionZoneController.detectedObjs.Count <= 0)
        {
            float dirchange = Random.Range(0f, dirChangeChance);

            if(dirchange < 10f) {
                dirChangeChance = 1000;
                float random = Random.Range(0f, 260f);
                direction = new Vector2(Mathf.Sin(random), Mathf.Cos(random)).normalized;
            }
            else
            {
                dirChangeChance = dirChangeChance - 1;
                rb.AddForce(direction * moveSpeed);

            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.takeDamage(attackPower, this);
            Debug.Log("TEST");
        }
    }
}

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
        isMoving = false;

        if (DetectionZoneController.detectedObjs.Count > 0)
        {
            if (DetectionZoneController.detectedObjs[0] != null && health > 0)
            {
                isMoving = true;
                GameObject target = DetectionZoneController.detectedObjs[0];

                Vector2 directionToTarget = (target.transform.position - transform.position).normalized;

                if (directionToTarget.x > 0f)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }

                rb.AddForce(directionToTarget * movemetSpeed);

                isMoving = true;
            }
        }
        animator.SetBool("IsMoving", isMoving);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        damagePlayer(collision.collider);
    }
}

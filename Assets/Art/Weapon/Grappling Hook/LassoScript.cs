using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LassoScript : MonoBehaviour
{
    public GameObject Claw;
    public float clawSpeed;
    public float maxClawDistance = 2f;
    public Transform shootPoint;
    public bool isAttached;
    public bool isMoving;

    Vector2 Direction;
    GameObject hitTarget;

    public LineRenderer rope;
    void Start()
    {
        rope.enabled = false;
        isAttached = false;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Direction = mousePos - (Vector2)transform.position;
        FaceMouse();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
        if(hitTarget != null)
        {
            rope.SetPosition(0, shootPoint.position);
            rope.SetPosition(1, hitTarget.transform.position);
        }
        float distance = Vector2.Distance(shootPoint.position, Claw.transform.position);
        if (distance > maxClawDistance)
        {
            RecallClaw();
        }
        if(distance < 0.2f && isMoving)
        {
            StopClaw();
        }

    }

    void FaceMouse()
    {
        transform.right= Direction;
    }
    public void Attach()
    {
        ClawScript clawScript = Claw.GetComponent<ClawScript>();
        isAttached = clawScript.attached;
    }

    void Shoot()
    {
        if (Claw != null)
        {
            Rigidbody2D bulletRigidbody = Claw.GetComponent<Rigidbody2D>();
            ClawScript clawScript = Claw.GetComponent<ClawScript>();

            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = transform.right * clawSpeed;
            }
            if (clawScript != null)
            {
                hitTarget = Claw;
                rope.enabled = true;
                isAttached = clawScript.attached;
            }
        }
    }

    void RecallClaw()
    {
        Rigidbody2D bulletRigidbody = Claw.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = (shootPoint.position - hitTarget.transform.position).normalized * clawSpeed*2f;
        }
        isMoving = true;
    }
    void StopClaw()
    {
        Rigidbody2D bulletRigidbody = Claw.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = Vector2.zero;
            Claw.transform.position = shootPoint.position;
            rope.enabled = false;
            isAttached = false;
            hitTarget = null;
            isMoving = false;
        }
    }
}

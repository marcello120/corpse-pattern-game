using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LassoScript : MonoBehaviour
{
    public GameObject Claw;
    public float clawSpeed;
    public float maxClawDistance = 2f;
    public Transform shootPoint;
    public bool isAttached;
    public bool isMoving;
    public bool isComingBack;

    Vector2 Direction;
    GameObject hitTarget;

    public LineRenderer rope;
    public ClawScript clawScript;
    public Rigidbody2D bulletRigidbody;

    void Start()
    {
        rope.enabled = false;
        isAttached = false;
        isMoving = false;
        clawScript = Claw.GetComponent<ClawScript>();
        bulletRigidbody = Claw.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Direction = mousePos - (Vector2)transform.position;

        FaceMouse();

        if (hitTarget != null)
        {
            rope.SetPosition(0, shootPoint.position);
            rope.SetPosition(1, hitTarget.transform.position);
        }
        float distance = Vector2.Distance(shootPoint.position, Claw.transform.position);

        if (isMoving) //Ha kilottuk
        {
            if (distance > maxClawDistance && !isAttached) //Akkor nazzuk, hogy mikor kell visszahivni, de csak ha nincsmar rajta valami
            {
                RecallClaw();
            }
        }
        else
        {
            if (isComingBack) //ha mar jon vissza
            {
                if (!isAttached)
                {
                    RecallClaw();
                }

                if (isMoving && distance < 0.5f) //es eleg kozel van, akkor allitsuk meg
                {
                    StopClaw();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E) && !isAttached) //Ha nincs kilove es nem jon vissza és nincs attacholva, akkor loje ki
                {
                    Shoot();
                }
            }
        }
    }

    void FaceMouse()
    {
        transform.right= Direction;
    }
    public void Attach()
    {
        isAttached = true;
    }

    void Shoot()
    {
        if (Claw != null)
        {
            clawScript.Shot();
            isMoving = true;
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = transform.right * clawSpeed;
            }
            if (clawScript != null)
            {
                hitTarget = Claw;
                rope.enabled = true;
            }
        }
    }

    public void RecallClaw()
    {
        isComingBack= true;
        isMoving = false;

        Rigidbody2D bulletRigidbody = Claw.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = (shootPoint.position - hitTarget.transform.position).normalized * clawSpeed*2f;
        }
    }
    public void StopClaw()
    {
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = Vector2.zero;
            Claw.transform.position = shootPoint.position;

            // Parent the Claw to the shootPoint
            Claw.transform.SetParent(shootPoint);

            // Reset local position and rotation to maintain the original offset and rotation
            Claw.transform.localPosition = Vector3.zero;
            Claw.transform.localRotation = Quaternion.identity;

            rope.enabled = false;
            isAttached = false;
            hitTarget = null;
            isMoving = false;
            isComingBack= false;
            clawScript.NotShot();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoScript : MonoBehaviour
{
    public GameObject Bullet;
    public float bulletSpeed;
    public Transform shootPoint;

    Vector2 Direction;
    GameObject hitTarget;

    public LineRenderer rope;
    void Start()
    {
        rope.enabled = false;
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
    }

    void FaceMouse()
    {
        transform.right= Direction;
    }

    void Shoot()
    {
        GameObject bulletInstance = Instantiate(Bullet, shootPoint.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);

        ClawScript clawScript = bulletInstance.GetComponent<ClawScript>();
        if (clawScript != null)
        {
            hitTarget = bulletInstance;
            rope.enabled = true;
        }
    }
}

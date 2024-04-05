using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowWeapon : Weapon
{
    public GameObject arrow;
    public float chargeSpeed;
    public float launchForce;
    private float originalLaunchForce;
    public Transform shootPoint;

    public GameObject point;
    GameObject[] points;
    public int numOfPoints;
    public float spaceBetweenPoints;
    public float gravityScale;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        originalLaunchForce = launchForce;

        points = new GameObject[numOfPoints];
        for (int i = 0; i < numOfPoints; i++)
        {
            points[i] = Instantiate(point, shootPoint.position, Quaternion.identity);
            points[i].transform.parent= transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < numOfPoints; i++)
        {
            if(launchForce > 2.3)
            {
                Debug.Log("HELLO");
            }
            points[i].transform.localPosition = PointPosition(i * spaceBetweenPoints);
            Color objectColor = points[i].GetComponent<Renderer>().material.color;
            float fadeAmount = launchForce / (originalLaunchForce * 2) - 0.5f;

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            points[i].GetComponent<Renderer>().material.color = objectColor;
        }
    }

    public override void Charge(float amount)
    {
        launchForce = Mathf.Min(launchForce + (chargeSpeed * amount), 2f * originalLaunchForce);

    }

    public override void Attack()
    {
        Shoot();
    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;

        //reset launchforce and numofPoints
        launchForce = originalLaunchForce;
    }

    Vector2 PointPosition(float f)
    {
        Vector2 position = (Vector2.right * launchForce * f) + 0.5f * Physics2D.gravity * (f * f) * gravityScale;
        return position;
    }

    public override void HeavyAttack()
    {
        Shoot();
    }
}

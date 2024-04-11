using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowWeapon : Weapon
{
    public GameObject arrow;
    public float chargeSpeed;
    public float launchForce;
    public float shootCoolDown = 3f;
    private float shootTimeCount;
    private bool canShoot = true;
    private float originalLaunchForce;
    public Transform shootPoint;
    public GameObject arrowSprite;

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
        if(!canShoot) //Shoot CD
        {
            arrowSprite.SetActive(false);
            shootTimeCount += Time.deltaTime;
            if(shootTimeCount > shootCoolDown)
            {
                canShoot = true;
                shootTimeCount= 0;
            }
        }
        else
        {
            arrowSprite.SetActive(true);
        }

        for (int i = 0; i < numOfPoints; i++)
        {
            points[i].transform.localPosition = PointPosition(i * spaceBetweenPoints);
            Color objectColor = points[i].GetComponent<Renderer>().material.color;
            float fadeAmount = launchForce / (originalLaunchForce * 2) - 0.5f;

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            points[i].GetComponent<Renderer>().material.color = objectColor;
        }
    }

    public override void Charge(float amount)
    {
        if (canShoot)
        {
            launchForce = Mathf.Min(launchForce + (chargeSpeed * amount), 2f * originalLaunchForce);
        }
    }

    public override void Attack()
    {
        Shoot();
    }

    void Shoot()
    {
        if (canShoot)
        {
            GameObject newArrow = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;

            //reset launchforce and numofPoints
            launchForce = originalLaunchForce;

            canShoot= false;
        }
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrow;
    public float launchForce; //Ezt változtassuk a lenyomással, illetve a number a Points
    public Transform shootPoint;

    public GameObject point;
    GameObject[] points;
    public int numOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;

    public float gravityScale;

    // Start is called before the first frame update
    void Start()
    {
        points= new GameObject[numOfPoints];
        for(int i = 0; i < numOfPoints; i++)
        {
            points[i]= Instantiate(point, shootPoint.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition- bowPosition;
        transform.right = direction;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }

        for (int i = 0; i < numOfPoints; i++)
        {
            points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
    }

    void Shoot()
    {
        GameObject newArrow = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
    }

    Vector2 PointPosition(float f)
    {
        Vector2 position = (Vector2)shootPoint.position + (direction.normalized * launchForce* f) + 0.5f*Physics2D.gravity * (f*f) * gravityScale;
        return position;
    }
}

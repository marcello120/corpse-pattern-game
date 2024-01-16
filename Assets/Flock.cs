using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed;
    bool turning = false;

    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.movementLimit * 2);

        if (!b.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                                                FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            ResetFlockSpeed();

            if (Random.Range(0, 100) < FlockManager.FM.goalMoveChance + 10f)
            {
                ApplyRules();
            }
        }

        this.transform.Translate(0, speed * Time.deltaTime, 0);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.FM.allFlock;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach(GameObject go in gos)
        {
            if(go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if(nDistance <= FlockManager.FM.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if(nDistance < 0.2f) //1 volt előtte
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if(groupSize> 0)
        {
            vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            if(speed > FlockManager.FM.maxSpeed) 
            {
                speed = FlockManager.FM.maxSpeed;
            }


            Vector3 direction = (vcentre + vavoid) - transform.position;
            if(direction != Vector3.zero)
            {
                transform.rotation= Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    FlockManager.FM.rotationSpeed*Time.deltaTime);
            }
        }
    }

    void ResetFlockSpeed()
    {
        if (Random.Range(0, 100) < FlockManager.FM.goalMoveChance + 5f)
        {
            speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public GameObject flockEnemy;
    public int numFlock = 10;
    public GameObject[] allFlock;
    public Vector3 movementLimit = new Vector3(1,1,0);
    public Vector3 goalPos = Vector3.zero;
    public float goalMoveChance = 10f;

    [Header("Flock Enemy Settings")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(1f, 10f)]
    public float neighbourDistance;
    [Range(0.5f, 5f)]
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        allFlock= new GameObject[numFlock];
        for (int i = 0; i < numFlock; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-movementLimit.x, movementLimit.x),
                                                                Random.Range(-movementLimit.y, movementLimit.y),
                                                                Random.Range(-movementLimit.z, movementLimit.z));

            allFlock[i] = Instantiate(flockEnemy, pos, Quaternion.identity);
        }
        FM= this;
        goalPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100) < goalMoveChance)
        {
            goalPos = this.transform.position + new Vector3(Random.Range(-movementLimit.x, movementLimit.x),
                                                                Random.Range(-movementLimit.y, movementLimit.y),
                                                                Random.Range(-movementLimit.z, movementLimit.z));
        }
    }
}

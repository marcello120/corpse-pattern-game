using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SnakeBoss : Enemy
{
    public GameObject segment;

    public List<GameObject> segmentList = new List<GameObject>();

    public int initialSize = 5;

    public float segmentDistance = 0.5f;

    public float frequency = 20f; // Speed of sine movement
    public float magnitude = 0.03f; //  Size of sine movement

    public Vector2 dirdir;

    public void Start()
    {
        base.Init();
        setState(State.Moving);
        target = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject newSegment = Instantiate(segment);

            if (segmentList.Count == 0)
            {
                newSegment.transform.position = transform.position - transform.right* segmentDistance;
            }
            else
            {
                newSegment.transform.position = segmentList[segmentList.Count - 1].transform.position - transform.right * segmentDistance;

            }
            segmentList.Add(newSegment);
        }

    }

    public override void moveInDirection(Vector3 direction)
    {

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.fixedDeltaTime);
        Vector3 dirToPlayer =  (target.position- transform.position).normalized;

        dirdir = new Vector2(dirToPlayer.x, dirToPlayer.y);


        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);
        //transform.rotation = rotation;


        // Apply the movement force to the Rigidbody
        Vector3 force = direction * movemetSpeed * 1f * Time.fixedDeltaTime;
        rb.AddForce(force);

    }

    public void FixedUpdate()
    {
        commonUpdate();

        for (int i = 0; i < segmentList.Count; i++)
        {
            Transform targetTransfrom = transform;
            GameObject currentSegment = segmentList[i];
            if (i != 0)
            {
                targetTransfrom = segmentList[i - 1].transform;
            }

            Vector3 dir = (targetTransfrom.position - currentSegment.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            currentSegment.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);

            if (Vector3.Distance(targetTransfrom.position, currentSegment.transform.position) > segmentDistance || i==0)
            {
                currentSegment.transform.position = Vector2.Lerp(currentSegment.transform.position, targetTransfrom.position, 4f * Time.fixedDeltaTime);
                //currentSegment.transform.position = Vector3.MoveTowards(currentSegment.transform.position, targetTransfrom.position, 2f * Time.fixedDeltaTime);
            }
        }
    }

}

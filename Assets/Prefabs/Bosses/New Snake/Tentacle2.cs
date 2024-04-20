using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tentacle2 : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;

    public Transform[] bodyParts;


    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    void Update()
    {
        //wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.deltaTime * wiggleSpeed) * wiggleMagnitude);

        ManageSnakeBody();

        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            bodyParts[i - 1].transform.position = segmentPoses[i];
        }
        lineRend.SetPositions(segmentPoses);
    }
    void ManageSnakeBody()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (bodyParts[i] == null)
            {
                //bodyParts[i] = bodyParts[i + 1];
                //bodyParts[i] = null; !!!VESZ�LYES!!!
            }
        }
        
    }
}

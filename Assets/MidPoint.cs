using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidPoint : MonoBehaviour
{
   public Transform playerTransform;
    public Camera mainCamera;
    public float cameraTargetDivider;
    public float maximumDistance;

    public bool overriden = false;

    private void Update()
    {
        if (overriden)
        {
            return;
        }
        if(playerTransform== null)
        {
            return;
        }
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cameraTargetPosition = (mousePosition + (cameraTargetDivider - 1) * playerTransform.position) / cameraTargetDivider;

        // Calculate the distance between the MidPoint and the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // If the MidPoint is further from the player than the maximum distance, move it closer to the player
        if (distanceToPlayer > maximumDistance)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.position = playerTransform.position - directionToPlayer * maximumDistance;
        }
        else
        {
            transform.position = cameraTargetPosition;
        }
    }

    public void setPos(Vector3 pos)
    {
        overriden = true;
        transform.position = pos;
    }

    public void release()
    {
        overriden= false;
    }
}

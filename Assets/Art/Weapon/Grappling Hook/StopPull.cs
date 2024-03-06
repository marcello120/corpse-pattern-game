using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPull : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the Claw GameObject
        if (other.gameObject.name == "Claw")
        {
            LassoScript lassoScript = other.GetComponentInParent<LassoScript>();

            if (lassoScript != null)
            {
                lassoScript.StopClaw();
            }
        }
    }
}

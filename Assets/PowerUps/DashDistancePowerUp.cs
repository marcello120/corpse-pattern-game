using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Dash Distance")]
public class DashDistancePowerUp : PowerUp
{
    public float dashDistanceIncrease;

    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.increaseDashAmount(dashDistanceIncrease);
            playerController.addPowerUp(this);
            return true;
        }
        else
        {
            return false;
        }
    }
}

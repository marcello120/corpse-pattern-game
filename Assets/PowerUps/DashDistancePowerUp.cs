using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Dash Distance")]
public class DashDistancePowerUp : PowerUp
{
    public float dashDistanceIncrease;

    public override void apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.increaseDashAmount(dashDistanceIncrease);
        }
    }
}

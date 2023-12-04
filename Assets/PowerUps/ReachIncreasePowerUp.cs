using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/Reach")]
public class ReachIncreasePowerUp : PowerUp
{
    public float reachIncreaseAmount;


    public override void apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.inreaseReach(reachIncreaseAmount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/MaxHPUp")]
public class MaxHPPowerUp : PowerUp
{
    public float maxHPIncreasAmount;

    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>()!= null )
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.inceaseMaxHealth(maxHPIncreasAmount);
            playerController.addPowerUp(this);
            return true;
        }
        else
        {
            return false;
        }
    }
}

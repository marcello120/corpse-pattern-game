using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Heal")]
public class HealPowerUp : PowerUp
{
    public float healAmount;

    public override bool apply(GameObject player)
    {
       if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>()!= null )
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.heal(healAmount);
            return true;
        }
        else
        {
            return false;
        }
    }
}

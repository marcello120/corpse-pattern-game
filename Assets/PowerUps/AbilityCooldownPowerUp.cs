using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/AbilityCooldown")]

public class AbilityCooldownPowerUp : PowerUp
{
    public float abilityCooldownDecreaseAmount;

    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.decreaseAbilityCooldown(abilityCooldownDecreaseAmount);
            return true;
        }
        else
        {
            Debug.Log("WHY?");
            return false;
        }
    }
}

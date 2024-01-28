using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/StatusDuration")]

public class StatusEffectDurationUp : PowerUp
{

    public float durationIncreaseAmount;

    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.increaseEffectsDuration(durationIncreaseAmount);
            return true;
        }
        return false;
    }


}

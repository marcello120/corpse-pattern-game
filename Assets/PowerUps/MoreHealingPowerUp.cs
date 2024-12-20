using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(menuName = "PowerUps/MoreHealing")]
    public class MoreHealingPowerUp : PowerUp
    {
        public int extraHealingIncreasAmount;

        public override bool apply(GameObject player)
        {
            if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
            {
                RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
                playerController.increasExtraHealing(extraHealingIncreasAmount);
                playerController.addPowerUp(this);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

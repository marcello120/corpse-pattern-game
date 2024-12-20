using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Doubler")]
public class SpawnDoublerPowerUp : PowerUp
{
    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            GameManager.Instance.SpawnDoubler();
            playerController.addPowerUp(this);
            return true;
        }
        return false;
    }
}

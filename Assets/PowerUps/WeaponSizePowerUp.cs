using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PowerUps/WeaponSize")]
public class WeaponSizePowerUp : PowerUp
{
    public float weaponSizeIncreaseAmount;

    public override bool apply(GameObject player)
    {
        if (player.tag == "Player" && player.GetComponent<RiggedPlayerController>() != null)
        {
            RiggedPlayerController playerController = player.GetComponent<RiggedPlayerController>();
            playerController.inreaseWeaponSize(weaponSizeIncreaseAmount);
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpListItem : MonoBehaviour
{
    public Image icon;
    public Image outline;
    // Start is called before the first frame update

    public void Init(PowerUp powerUp)
    {
        icon.sprite = powerUp.sprite;
        outline.sprite = powerUp.sprite;
        outline.color = powerUp.powerUpColor;
    }

}

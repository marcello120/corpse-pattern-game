using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopItemUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public Image background;
    public Image icon;

    public Color activeColor;
    public Color purchasedColor;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(ShopManager.ShopItem shopItem) 
    {
        title.SetText(shopItem.name);
        description.SetText(shopItem.description);
        if (shopItem.purchased)
        {
            background.color = purchasedColor;
        }
        if (shopItem.isActive)
        {
            background.color = activeColor;
        }
    }
}

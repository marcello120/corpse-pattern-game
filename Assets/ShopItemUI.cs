using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;


public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI price;
    public Image background;
    public Image icon;
    public Button button;

    public Color activeColor;
    public Color purchasedColor;
    public Color poorColor;


    public ShopManager.ShopItem shopItem;

    public ShopManager shopManager;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { choose(); });
    }

    public void choose()
    {
        int totalScoreSaved = PlayerPrefs.GetInt("TotalScore", 0);
        if ((totalScoreSaved < shopItem.cost) && !shopItem.purchased)
        {
            return;
        }
        shopManager.choose(shopItem);
    }

    public void Init(ShopManager.ShopItem shopItem, ShopManager shopManager ) 
    {
        this.shopItem = shopItem;
        this.shopManager = shopManager;
        title.SetText(shopItem.name);
        description.SetText(shopItem.description);
        price.SetText(shopItem.cost.ToString());
        int totalScoreSaved = PlayerPrefs.GetInt("TotalScore", 0);
        if((totalScoreSaved < shopItem.cost) && !shopItem.purchased) 
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
            background.color = poorColor;

        }
        if (shopItem.purchased)
        {
            background.color = purchasedColor;
            price.SetText("");
        }
        if (shopItem.isActive)
        {
            price.SetText("");
            background.color = activeColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int totalScoreSaved = PlayerPrefs.GetInt("TotalScore", 0);
        if ((totalScoreSaved < shopItem.cost) && !shopItem.purchased)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = Vector3.one * 1.1f;

        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;

    }
}

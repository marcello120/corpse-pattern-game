using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static RiggedPlayerController;

public class ShopManager : MonoBehaviour
{
    [Serializable]
    public class ShopItem
    {
        public string name;
        public Utility utility;
        public string description;
        public int cost;
        public bool isActive;
        public bool purchased;

        public ShopItem(string nameIn, Utility utilityIn, string descriptionIn, int costIn, bool isActiveIn, bool purchasedIn)
        {
            name = nameIn;
            utility = utilityIn;
            description = descriptionIn;
            cost = costIn;
            isActive = isActiveIn;  
            purchased = purchasedIn; 
        }

    }


    public List<ShopItem> shopItems = new List<ShopItem>();
    public GameObject shopCanvas;
    public GameObject shopItemUi;

    public RiggedPlayerController player;
    public CinemachineVirtualCamera cinemachineCamera;



    void Start()
    {
        //seed();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<RiggedPlayerController>();
        init();
    }

    private void init()
    {
        foreach (Transform child in shopCanvas.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < shopItems.Count; i++)
        {
            ShopItem item = shopItems[i];
            int active = PlayerPrefs.GetInt(item.utility.ToString() + "_isActive", 0);
            if (active == 1)
            {
                item.isActive = true;
            }
            else
            {
                item.isActive = false;
            }
            int purch = PlayerPrefs.GetInt(item.utility.ToString() + "_purchased", 0);
            if (purch == 1)
            {
                item.purchased = true;
            }
            else
            {
                item.purchased = false;
            }

            GameObject uiElement = Instantiate(shopItemUi, shopCanvas.transform);
            uiElement.GetComponent<ShopItemUI>().Init(item,this);
            float shopItemHight = 70f;
            float offset = -230;
            int index = i;
            if (i > 3)
            {
                offset *= -1;
                index -= 4;
            }
            uiElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 100 - (shopItemHight * index));

        }
    }

    private void seed()
    {
        shopItems.Clear();
        Utility[] utils = (Utility[])Enum.GetValues(typeof(Utility));
        foreach (Utility util in utils)
        {
            if (util != Utility.NONE)
            {
                shopItems.Add(new ShopItem(ConvertToTitleCase(util.ToString()), util, ConvertToTitleCase(util.ToString()), 500, false, false));
            }
        }
    }

    public void choose(ShopItem shopItem)
    {
        Debug.Log("Hellp");
        if (!shopItem.purchased)
        {
            GameManager.changeTotal(shopItem.cost * -1);
            PlayerPrefs.SetInt(shopItem.utility.ToString() + "_purchased", 1);
            PlayerPrefs.SetInt(shopItem.utility.ToString() + "_isActive", 1);
        }
        if (!shopItem.isActive && shopItem.purchased) 
        {
            PlayerPrefs.SetInt(shopItem.utility.ToString() + "_isActive", 1);
        }
        foreach (ShopItem item in shopItems)
        {
            if (item.utility != shopItem.utility)
            {
                PlayerPrefs.SetInt(item.utility.ToString() + "_isActive", 0);
            }
        }
        player.selectedUtility= shopItem.utility;

        init();

    }

    public static string ConvertToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        string[] words = input.Split('_');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
        }

        return string.Join(" ", words);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Initialize the shop
            init();

            // Enable shopCanvas
            shopCanvas.SetActive(true);

            // Gradually show shopCanvas (fade-in effect)
            StartCoroutine(FadeCanvasGroup(shopCanvas.GetComponent<CanvasGroup>(), 0f, 1f, 1f));

            // Make the main camera zoom in on the player
            StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 1f, 0.5f)); // Adjust target zoom and duration as needed
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Gradually make shopCanvas disappear (fade-out effect)
            StartCoroutine(FadeCanvasGroup(shopCanvas.GetComponent<CanvasGroup>(), 1f, 0f, 1f, () =>
            {
                // Disable shopCanvas after fading out
                shopCanvas.SetActive(false);
            }));

            // Restore main camera to original view
            StartCoroutine(CinemachineCameraZoom(cinemachineCamera, 2f, 0.5f)); // Adjust target zoom and duration as needed
        }
    }

    // Helper method for fading CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration, Action onComplete = null)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (onComplete != null)
        {
            onComplete();
        }
    }

    // Helper method for camera zoom
    private IEnumerator CinemachineCameraZoom(CinemachineVirtualCamera virtualCamera, float targetZoom, float duration)
    {
        if (virtualCamera == null) yield break;

        float startZoom = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startZoom, targetZoom, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetZoom;
    }



}

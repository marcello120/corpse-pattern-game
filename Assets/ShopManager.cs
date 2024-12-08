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


    // Start is called before the first frame update
    void Start()
    {
        foreach(ShopItem item in shopItems)
        {
           int active = PlayerPrefs.GetInt(item.utility.ToString() + "_isActive",0);
            if (active==1)
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
        }
    }

    private void init()
    {
        Utility[] utils = (Utility[])Enum.GetValues(typeof(Utility));
        foreach (Utility util in utils)
        {
            if (util != Utility.NONE)
            {
                shopItems.Add(new ShopItem(util.ToString(), util, util.ToString(), 500, false, false));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hello");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpList : MonoBehaviour
{
    public GameObject powerUpListItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPowerUp(PowerUp power)
    {
        GameObject newItem = Instantiate(powerUpListItem, transform);
        newItem.GetComponent <PowerUpListItem>().Init(power);
    }

}

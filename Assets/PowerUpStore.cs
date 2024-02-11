using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpStore : MonoBehaviour
{
    public static PowerUpStore instance;

    public PowerUp[] powerUps;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public PowerUp[] getThreeRandomPowerUps()
    {
        List<PowerUp> shuffledList = getShuffledList(powerUps);
        return new PowerUp[] { shuffledList[0], shuffledList[1], shuffledList[2] };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<PowerUp> getShuffledList(PowerUp[] puwerUpArray)
    {
        System.Random random= new System.Random();
        List<PowerUp> powerUpList = new List<PowerUp>(puwerUpArray);
        int n = powerUpList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            PowerUp value = powerUpList[k];
            powerUpList[k] = powerUpList[n];
            powerUpList[n] = value;
        }

        return powerUpList;
    }
}

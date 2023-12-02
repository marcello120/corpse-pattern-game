using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSelection : MonoBehaviour
{
    public UI_PowerUpOption option1;
    public UI_PowerUpOption option2;
    public UI_PowerUpOption option3;

    // Start is called before the first frame update
    void Start()
    {
        show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
        }
        gameObject.SetActive(true);
        PowerUp[] powerUps = PowerUpStore.instance.getThreeRandomPowerUps();
        option1.powerUp= powerUps[0];
        option1.init();
        option2.powerUp= powerUps[1];
        option2.init();
        option3.powerUp= powerUps[2];
        option3.init();
    }

    public void hide()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        gameObject.SetActive(false);
    }



}

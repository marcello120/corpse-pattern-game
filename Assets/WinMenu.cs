using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WinMenu : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI corpsesOnScreen;
    public TextMeshProUGUI timeBonus;
    public TextMeshProUGUI hpBonus;
    public TextMeshProUGUI finalScore;


    public void refresh(int score, int corpses, int time, int hp, int final)
    {
        scoreText.text = (score*10).ToString();
        corpsesOnScreen.text = (-1*corpses).ToString();
        timeBonus.text = time.ToString();
        hpBonus.text = hp.ToString();
        finalScore.text = final.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

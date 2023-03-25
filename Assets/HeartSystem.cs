using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    
    public float hearts;
    public int heartCount;

    public Image[] lives;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public PlayerController playercontroller;

    void Update()
    {
        hearts = playercontroller.playerHealth;

        if (hearts>heartCount)
        {
            hearts = heartCount;
        }

        for(int i = 0; i < lives.Length; i++) 
        {
            if(i<hearts) 
            {
                lives[i].sprite= fullHeart;
            }
            else
            {
                lives[i].sprite= emptyHeart;
            }

            if(i<heartCount)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        heartCount -= damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObject : MonoBehaviour
{
    public PowerUp powerUp;

    public SpriteRenderer sprite;

    void Start()
    {
        init(powerUp);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            powerUp.apply(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void init(PowerUp powerUpIn)
    {
        sprite = GetComponent<SpriteRenderer>();
        powerUp = powerUpIn;
        sprite.color = powerUp.powerUpColor;
    }
}
 

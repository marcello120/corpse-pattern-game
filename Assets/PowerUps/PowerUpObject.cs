using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerUpObject : MonoBehaviour
{
    public PowerUp powerUp;

    public SpriteRenderer sprite;

    public Rigidbody2D rb;

    public Light2D light;

    void Start()
    {
        init(powerUp);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            powerUp.apply(collision.gameObject);
        }
    }

    public void init(PowerUp powerUpIn)
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        powerUp = powerUpIn;
        //sprite.color = powerUp.powerUpColor;
        sprite.sprite = powerUp.sprite;
        light = GetComponentInChildren<Light2D>();
        light.color = powerUp.powerUpColor;
    }
}
 

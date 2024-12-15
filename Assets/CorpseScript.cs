using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class CorpseScript : MonoBehaviour
{
    public int corpseNumber;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public SlowStatusEffect slowStatus;
    public Light2D light2d;
    public Light2D bigLight;


    static Color GREEN_COLOR = new Color(0f, 1f, 0f); // Pure green
    static Color BLUE_COLOR = new Color(0f, 0f, 1f); // Pure blue
    static Color PURPLE_COLOR = new Color(1f, 0f, 1f); // Medium purple
    static Color ORANGE_COLOR = new Color(1f, 0.5f, 0f); // Orange
    static Color RED_COLOR = new Color(1f, 0f, 0f); // Pure red



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        Color color = getColorFromCorpseCode(corpseNumber);
        light2d.color = color;
        if(GameManager.Instance.currentLevel== GameManager.Level.DARKNESS) {
            bigLight.enabled = true;
        }
        else
        {
            bigLight.enabled = false;
        }
    }

    public void Init(int coprseNumIn)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        corpseNumber = coprseNumIn;
        spriteRenderer.sprite = CorpseStore.Instance.configs[corpseNumber];



        if (corpseNumber == 99)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private Color getColorFromCorpseCode(int corpseNumber)
    {
        switch (corpseNumber / 10)
        {
            case 1:
                return Color.gray;
            case 2:
                return GREEN_COLOR;
            case 3:
                return BLUE_COLOR;
            case 4:
                return PURPLE_COLOR;
            case 5:
                return ORANGE_COLOR;
            case 6:
                return RED_COLOR;
            default:
                return Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (corpseNumber == 99)
        {
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<RiggedPlayerController>().Slow();
            }
            if (collision.tag == "Enemy")
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null && !enemy.isSlowed())
                {
                    enemy.addStatusEffect(Instantiate(slowStatus));

                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (corpseNumber == 99) {
            if (collision.tag == "Player")
            {
                collision.gameObject.GetComponent<RiggedPlayerController>().UnSlow();
            }
        }
    }

    public void Remove()
    {
        animator.SetTrigger("Die");
    }
}

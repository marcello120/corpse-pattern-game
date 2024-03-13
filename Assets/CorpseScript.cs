using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseScript : MonoBehaviour
{
    public int corpseNumber;

    Animator animator;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer= GetComponent<SpriteRenderer>();
    }

    public void Init(int coprseNumIn)
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        corpseNumber = coprseNumIn;
        spriteRenderer.sprite = PatternStore.Instance.configs[corpseNumber];

        if (corpseNumber == 99)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 0;
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
        if(collision.tag == "Player" && corpseNumber == 99)
        {
            collision.gameObject.GetComponent<RiggedPlayerController>().Slow();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && corpseNumber == 99)
        {
            collision.gameObject.GetComponent<RiggedPlayerController>().UnSlow();
        }
    }

    public void Remove()
    {
        animator.SetTrigger("Die");
    }
}

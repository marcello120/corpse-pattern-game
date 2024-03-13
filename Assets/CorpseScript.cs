using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseScript : MonoBehaviour
{
    public int corpseNumber;

    Animator animator;
    SpriteRenderer spriteRenderer;
    public SlowStatusEffect slowStatus;
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

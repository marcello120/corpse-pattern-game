using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseScript : MonoBehaviour
{
    public int corpseNumber;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Init(int coprseNumIn)
    {
        corpseNumber = coprseNumIn;
        if(corpseNumber == 99)
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

    public void Remove()
    {
        animator.SetTrigger("Die");
    }
}

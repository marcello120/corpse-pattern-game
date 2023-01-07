using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CorpseCleaner : MonoBehaviour
{
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
       circleCollider = GetComponent<CircleCollider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log( "Coll: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Corpse")
        {
            StartCoroutine(WaitAndDestory(collision));
        }

    }

    private IEnumerator WaitAndDestory(Collider2D collision)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}

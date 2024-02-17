using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColDetector : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> obstaceles = new List<GameObject>();


    Collider2D colliderChekcer;
    // Start is called before the first frame update
    void Start()
    {
        colliderChekcer = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.layer == 17)
        {
            Debug.Log("ENTERED COLLIDER");
            enemies.Add(collision.gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.layer == 17)
        {
            Debug.Log("Exit COLLIDER");
            enemies.Remove(collision.gameObject);
        }
    }
}

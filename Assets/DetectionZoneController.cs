using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class DetectionZoneController : MonoBehaviour
{
    public List<GameObject> detectedObjs = new List<GameObject>();

    CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
      circleCollider2D = GetComponent<CircleCollider2D>();  
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            detectedObjs.Add(collision.gameObject);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            detectedObjs.Remove(collision.gameObject);
        }
    }
}

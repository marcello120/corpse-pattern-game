using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDetectionZone : MonoBehaviour
{
    public RiggedPlayerController player;

    CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<RiggedPlayerController>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = null;
        }
    }
}

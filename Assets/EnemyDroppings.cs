using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroppings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Rigidbody2D child in gameObject.GetComponentsInChildren<Rigidbody2D>())
        {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            child.AddForce(direction * 1.5f);
        }
        foreach (SpriteRenderer child in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            // Get the current color of the sprite
            Color color = child.color;

            // Set the alpha to 75% (0.75f)
            color.a = 0.75f;

            // Apply the modified color back to the sprite
            child.color = color;
        }
        Destroy(gameObject,100f);
    }

}

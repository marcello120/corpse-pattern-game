using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public bool shouldSelfDestruct;
    public float destructTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        if (shouldSelfDestruct)
        {
            Destroy(gameObject, destructTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

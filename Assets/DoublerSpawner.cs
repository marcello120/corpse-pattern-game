using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DoublerSpawner : MonoBehaviour
{
    public float radius;
    public Doubler doublerPrefab;

    public Doubler SpawnDoubler()
    {
        float xoffset = UnityEngine.Random.Range(-radius, radius);
        float yoffset = UnityEngine.Random.Range(-radius, radius);
        Vector2 randomPoint = new Vector2(transform.position.x + xoffset, transform.position.y +  yoffset);
        return Instantiate(doublerPrefab, Grid.adjustWoldPosToNearestCell(randomPoint, 0.5f), Quaternion.identity);

    }

    // Start is called before the first frame update
    void Start()
    {


    }

}

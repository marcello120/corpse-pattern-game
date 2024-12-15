using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using ChristinaCreatesGames.Animations;
using Unity.VisualScripting.Dependencies.Sqlite;

public class Destructible : MonoBehaviour
{
    public float health;
    public GameObject hitEffect;
    public bool blocking;
    public AudioClip hitSound;
    public AudioClip breakSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float hitDestructible(float damage)
    {
        health -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        SquashAndStretch squasheffect = GetComponent<SquashAndStretch>();
        if(squasheffect != null)
        {
            squasheffect.PlaySquashAndStretch();
        }
        audioSource.PlayOneShot(hitSound);
        if (health < 1)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            if (blocking)
            {
                var guo = new GraphUpdateObject(GetComponent<Collider2D>().bounds);
                guo.updatePhysics = true;
                AstarPath.active.UpdateGraphs(guo);
            }
            Destroy(gameObject);

        }
        return health;
    }

}

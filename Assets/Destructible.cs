using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using ChristinaCreatesGames.Animations;

public class Destructible : MonoBehaviour
{
    public float health;
    public bool dmgOnDash = false;
    public GameObject hitEffect;
    public bool blocking;
    public AudioClip hitSound;
    public AudioClip breakSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        if (GetComponent<Explodable>() != null)
        {
            GetComponent<Explodable>().allowRuntimeFragmentation = true;
            GetComponent<Explodable>().withForce = true;

        }
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
        if(audioSource!= null && hitSound!=null) {
            audioSource.PlayOneShot(hitSound);
        }
        if (health < 1)
        {
            if (audioSource != null && breakSound != null)
            {
                AudioSource.PlayClipAtPoint(breakSound, transform.position);
            }
            if (GetComponent<Explodable>() != null) {
                GetComponent<Explodable>().explode();
            }
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

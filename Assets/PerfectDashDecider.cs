using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PerfectDashDecider : MonoBehaviour
{
    // Start is called before the first frame update

    public ColDetector redZone;
    public ColDetector greenZone;

    public RiggedPlayerController player;

    public AudioSource audioSource;

    private static float MAGIC_NUMBER = 1f / 0.07f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = transform.parent.GetComponent<RiggedPlayerController>();
        redZone.transform.localScale = new Vector3(player.DashAmount * MAGIC_NUMBER, redZone.transform.localScale.y);
        float redOffset = (player.DashAmount * MAGIC_NUMBER) / 2f;
        redZone.transform.localPosition = new Vector3(redOffset , redZone.transform.localPosition.y);
        greenZone.transform.localPosition = new Vector3((player.DashAmount) * MAGIC_NUMBER+ 0.5f, greenZone.transform.localPosition.y);

    }

    public void playNice()
    {
        audioSource.Play();
    }

    public List<GameObject> isPerfect()
    {
        //red has enemy and green has no enemy
        if (redZone.enemies.Count > 0 && greenZone.enemies.Count <= 0)
        {
            return redZone.enemies;
        }
        return new List<GameObject>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

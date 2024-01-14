using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DamageTrail : MonoBehaviour
{

    public GameObject track;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnTrail), 0f, 0.4f); // Spawn Trail 

    }

    private void SpawnTrail()
    {
        GameObject spawnedTrack = Instantiate(track, transform.position, Quaternion.identity);
        DamageSurface damageSurface = spawnedTrack.GetComponent<DamageSurface>();
        damageSurface.parent = GetComponent<Enemy>();
        damageSurface.timeToLive = 10f;
        damageSurface.attackPower = 1f;
        damageSurface.lifeTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

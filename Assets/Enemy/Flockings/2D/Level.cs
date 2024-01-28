using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform memberPrefab;
    public Transform enemyPrefab;
    public int numMembers;
    public int numEnemies;
    public List<Member> members;
    public List<Enemy> enemies;
    public float bounds;
    public Vector3 spawnRadius = new Vector3(1, 1, 0);
    public Vector3 localPos;

    // Start is called before the first frame update
    void Start()
    {
        localPos = this.transform.position;
        members = new List<Member>();
        enemies = new List<Enemy>();

        Spawn(memberPrefab, numMembers);
        Spawn(enemyPrefab, numEnemies);

        members.AddRange(FindObjectsOfType<Member>());
        enemies.AddRange(FindObjectsOfType<Enemy>());
    }

    void Spawn(Transform prefab, int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-spawnRadius.x, spawnRadius.x), Random.Range(-spawnRadius.y, spawnRadius.y), 0);
            
            Instantiate(prefab, pos, Quaternion.identity); 
        }
    }

    public List<Member> GetNeighbors(Member member, float radius)
    {
        List<Member> neighborsFound = new List<Member>();
        foreach(var otherMember in members)
        {
            if (otherMember == member)
                continue;

            if(Vector3.Distance(member.transform.position, otherMember.transform.position) <= radius)
            {
                neighborsFound.Add(otherMember);
            }
        }

        return neighborsFound;
    }

    public List<Enemy> GetEnemies(Member member, float radius)
    {
        List<Enemy> returnEnemies = new List<Enemy>();
        foreach(var enemy in enemies)
        {
            if(Vector3.Distance(member.position, enemy.transform.position) <= radius)
            {
                returnEnemies.Add(enemy);
            }
        }
        return returnEnemies;
    }
}

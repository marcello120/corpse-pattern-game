using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStore : MonoBehaviour
{

    public static EnemyStore instance;

    public Enemy[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Enemy getRandomEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemies.Length);

        return enemies[randomIndex];
    }

    public Enemy getRandomEnemyWithMaxPower(int maxPower)
    {
        int randomSkipChance = UnityEngine.Random.Range(1, maxPower+1);

        if(randomSkipChance > 1) {
            return getRandomEnemyWithMaxPower(maxPower - 1);

        }

        // Filter the enemies array to get only enemies with powerLevel equal to maxPower
        var eligibleEnemies = enemies.Where(enemy => enemy.powerLevel == maxPower).ToList();

        // Check if there are eligible enemies
        if (eligibleEnemies.Count > 0)
        {
            // Select a random enemy from the filtered list
            int randomIndex = UnityEngine.Random.Range(0, eligibleEnemies.Count);
            return eligibleEnemies[randomIndex];
        }
        else
        {
            // If no eligible enemies found, return null or handle appropriately
            return getRandomEnemyWithMaxPower(maxPower-1);
        }
    }

    public List<Enemy> getShuffledList(Enemy[] enemyArray)
    {
        System.Random random = new System.Random();
        List<Enemy> enemyList = new List<Enemy>(enemyArray);
        int n = enemyList.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Enemy value = enemyList[k];
            enemyList[k] = enemyList[n];
            enemyList[n] = value;
        }

        return enemyList;
    }

}

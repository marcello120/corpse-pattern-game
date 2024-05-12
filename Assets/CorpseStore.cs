using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseStore : MonoBehaviour
{

    public List<CorpseConfig> corpseConfigs;

    public Dictionary<int, Sprite> configs = new Dictionary<int, Sprite>();


    public static CorpseStore Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (var item in corpseConfigs)
        {
            configs.Add(item.key, item.corpse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] spiceItUp(int[,] inpattern, int spiceChance)
    {
        int rows = inpattern.GetLength(0);
        int cols = inpattern.GetLength(1);

        int[,] spiced = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (UnityEngine.Random.Range(1, spiceChance) == 1 && inpattern[i, j] != -1 && inpattern[i, j] != 0)
                {
                    CorpseConfig selected = corpseConfigs[UnityEngine.Random.Range(0, corpseConfigs.Count)];
                    if(selected.key < 100)
                    {
                        spiced[i, j] = inpattern[i, j];
                    }
                    spiced[i, j] = selected.key;
                }
                else
                {
                    spiced[i, j] = inpattern[i, j];
                }
            }
        }
        return spiced;
    }
}

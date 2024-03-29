using Pathfinding.Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public  class PatternStore: MonoBehaviour
{
    public List<CorpseConfig> corpseConfigs;

    public List<CorpsePattern> corpsePatterns2 = new List<CorpsePattern>
    {
          new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[1, 3]
            {
                {1,1,1}
            },
            "vertical line",
            null
        )
    };


    public List<CorpsePattern> corpsePatterns = new List<CorpsePattern>
    {
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[1, 3]
            {
                {1,1,1}
            },
            "vertical line",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[3, 1]
            {
                {1},
                {1},
                {1}
            },
            "horizontal line",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[2, 2]
            {
                {1,1},
                {1,-1}
            },
            "top right corner",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[2, 2]
            {
                {1,1},
                {-1,1}
            },
            "bottom right corner",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[2, 2]
            {
                {1,-1},
                {1,1}
            },
            "top left corner",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[2, 2]
            {
                {-1,1},
                {1,1}
            },
            "bottom left corner",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[5, 2]
            {
                {-1,1},
                {1,-1},
                {-1,1},
                {1,-1},
                {-1,1}
            },
            "wave",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[4, 2]
            {
                {1,1},
                {1,-1},
                {1,-1},
                {1,-1}
            },
            "snake",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[2, 3]
            {
                {1,1,1},
                {1,-1,-1}
            },
            "reed",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 3]
            {
                {-1,1,-1},
                {1,-1,1},
                {-1,1,-1}
            },
            "eye",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 3]
            {
                {1,-1,1},
                {-1,1,-1},
                {1,-1,1}
            },
            "star",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.HARD,
            new int[4, 4]
            {
                {1,1,1,1 },
                {1,-1,-1,1 },
                {1,-1,-1,1 },
                {1,1,1,1 }
            },
            "circle",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.HARD,
            new int[4, 4]
            {
                {-1,-1,1,-1 },
                {1,1,1,1 },
                {1,1,1,1 },
                {-1,-1,1,-1 }
            },
            "up arrow",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.HARD,
            new int[4, 4]
            {
                {-1,1,1,-1 },
                {-1,1,1,-1 },
                {1,1,1,1 },
                {-1,1,1,-1 }
            },
            "right arrow",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 3]
            {
                {1,1,1},
                {1,-1,-1},
                {1,1,1}
            },
            "u",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 3]
            {
                {1,-1,-1},
                {1,1,1},
                {1,-1,-1}
            },
            "fountain",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[2, 2]
            {
                {12,1},
                {-1,1}
            },
            "div2011",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[4, 3]
            {
                {-1,1,1 },
                {1,-1,1 },
                {1,-1,1 },
                {-1,1,1 }
            },
            "bowl",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 4]
            {
                {-1,-1,-1,1 },
                {1,1,1,1 },
                {-1,1,1,-1 }
            },
            "bird",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 4]
            {
                {1,-1,1,-1 },
                {-1,1,-1,1 },
                {1,-1,1,-1 }
            },
            "twist",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[3, 3]
            {
                {1,-1,-1},
                {1,-1,1},
                {1,1,-1}
            },
            "hand",
            null
        ),
        new CorpsePattern
        (
            CorpsePattern.Difficulty.MEDIUM,
            new int[4, 3]
            {
                {1,-1,-1 },
                {1,1,1 },
                {1,1,-1 },
                {1,1,-1 }
            },
            "sphinx",
            null
        ),
         new CorpsePattern
        (
            CorpsePattern.Difficulty.EASY,
            new int[3, 3]
            {
                {1,-1,1},
                {-1,1,-1},
                {1,-1,1}
            },
            "Diagonal Cross",
            null
        )
    };


    public Dictionary<int, Sprite> configs = new Dictionary<int, Sprite>();


    public static PatternStore Instance;

    [Serializable]
    public class CorpsePattern
    {
        public enum Difficulty {
            EASY,
            MEDIUM,
            HARD
        }

        public Difficulty difficulty;
        public int[,] pattern;
        public string name;
        public Sprite glif;

        public CorpsePattern( Difficulty difficulty, int[,] pattern, string name, Sprite glif)
        {
            this.difficulty = difficulty;
            this.pattern = pattern;
            this.name = name;
            this.glif = glif;
        }

    }

    public void Awake()
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

    public CorpsePattern GetRandomPatternWithDifficulty(CorpsePattern.Difficulty difficulty)
    {
        var hardPatterns = corpsePatterns.Where(pattern => pattern.difficulty == difficulty).ToList();

        if (hardPatterns.Count == 0)
        {
            return null;
        }

        return hardPatterns[UnityEngine.Random.Range(0, hardPatterns.Count)];
    }

    public CorpsePattern getRandomPattern()
    {
        return corpsePatterns[UnityEngine.Random.Range(0, corpsePatterns.Count)];
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
                if (UnityEngine.Random.Range(1, spiceChance) == 1 && inpattern[i, j]!= -1 && inpattern[i, j]!= 0)
                {
                    spiced[i, j] = corpseConfigs[UnityEngine.Random.Range(0, corpseConfigs.Count)].key;
                }
                else
                {
                    spiced[i, j] = inpattern[i, j];
                }
            }
        }
        return spiced;
    }

    public static int[,] Rotate(int[,] input)
    {
        int rows = input.GetLength(0);
        int cols = input.GetLength(1);

        int[,] rotated = new int[cols, rows];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                rotated[i, j] = input[j, cols - 1 - i];
            }
        }

        return rotated;
    }

}

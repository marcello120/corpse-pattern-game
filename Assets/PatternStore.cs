using Array2DEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class PatternStore: MonoBehaviour
{
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
        [SerializeField]
        public Array2DInt arrayInt;

        public CorpsePattern( Difficulty difficulty, int[,] pattern, string name, Sprite glif)
        {
            this.difficulty = difficulty;
            this.pattern = pattern;
            this.name = name;
            this.glif = glif;
            arrayInt = new Array2DInt(pattern);
        }

        public int[,] getPatternFrom2DArray()
        {
            return ReverseColumns(arrayInt.toIntArrary());
            //return arrayInt.toIntArrary();

        }

    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //foreach (var item in corpsePatterns)
        //{
        //    if(item.arrayInt.getGridSize() != Vector2Int.one)
        //    {
        //        item.pattern = item.arrayInt.toIntArrary();
        //    }
        //}


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

    public CorpsePattern GetPatternByName(string name)
    {
        var patternlist = corpsePatterns.Where(pattern => pattern.name == name).ToList();
        if(patternlist.Count == 0)
        {
            return null;
        }
        else
        {
            return patternlist[0];
        }

    }

    static int[,] ReverseColumns(int[,] array)
    {
        int rows = array.GetLength(0);
        int columns = array.GetLength(1);
        int[,] reversedArray = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                reversedArray[i, columns - 1 - j] = array[i, j];
            }
        }

        return reversedArray;
    }

    static int[,] ReverseRows(int[,] array)
    {
        int rows = array.GetLength(0);
        int columns = array.GetLength(1);
        int[,] reversedArray = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                reversedArray[rows - 1 - i, j] = array[i, j];
            }
        }

        return reversedArray;
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

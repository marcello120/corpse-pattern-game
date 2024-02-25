using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public  class PatternStore: MonoBehaviour
{
    public List<CorpseConfig> corpseConfigs;

    public Dictionary<int, Sprite> configs = new Dictionary<int, Sprite>();

    public List<int[,]> patterns;
    public List<int[,]> easyPatterns;
    public List<int[,]> mediumPatterns;
    public List<int[,]> hardPatterns;


    public static PatternStore Instance;


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



        patterns = new List<int[,]>();
        easyPatterns = new List<int[,]>();
        mediumPatterns= new List<int[,]>();
        hardPatterns = new List<int[,]>();


        int[,] verticalLine = new int[1, 3] {
            {1,1,1}
        };
        easyPatterns.Add(verticalLine);


        int[,] horizontalLine = new int[3, 1] {
            {1},
            {1},
            {1}
        };
        easyPatterns.Add(horizontalLine);

        int[,] toprightcorner = new int[2, 2] {
            {1,1},
            {1,-1}
        };
        easyPatterns.Add(toprightcorner);

        int[,] bottomrightcorner = new int[2, 2] {
            {1,1},
            {-1,1}
        };
        easyPatterns.Add(bottomrightcorner);

        int[,] topleftcorner = new int[2, 2] {
            {1,-1},
            {1,1}
        };
        easyPatterns.Add(topleftcorner);

        int[,] bottomleftcorner = new int[2, 2] {
            {-1,1},
            {1,1}
        };
        easyPatterns.Add(bottomleftcorner);

        int[,] wave = new int[5, 2] {
            {-1,1},
            {1,-1},
            {-1,1},
            {1,-1},
            {-1,1}
        };
        mediumPatterns.Add(wave);

        int[,] snake = new int[4, 2] {
            {1,1},
            {1,-1},
            {1,-1},
            {1,-1}
        };
        mediumPatterns.Add(snake);

        int[,] reed = new int[2, 3] {
            {1,1,1},
            {1,-1,-1}
        };
        mediumPatterns.Add(reed);

        int[,] eye = new int[3, 3] {
            {-1,1,-1},
            {1,-1,1},
            {-1,1,-1},
        };
        mediumPatterns.Add(eye);

        int[,] star = new int[3, 3] {
            {1,-1,1},
            {-1,1,-1},
            {1,-1,1},
        };
        mediumPatterns.Add(star);

        int[,] circle = new int[4, 4] {
            {1,1,1,1 },
            {1,-1,-1,1 },
            {1,-1,-1,1 },
            {1,1,1,1 }
        };
        hardPatterns.Add(circle);

        int[,] upArrow = new int[4, 4] {
            {-1,-1,1,-1 },
            {1,1,1,1 },
            {1,1,1,1 },
            {-1,-1,1,-1 }
        };
        hardPatterns.Add(upArrow);

        int[,] rightArrow = new int[4, 4] {
            {-1,1,1,-1 },
            {-1,1,1,-1 },
            {1,1,1,1 },
            {-1,1,1,-1 }
        };
        hardPatterns.Add(rightArrow);

        int[,] u = new int[3, 3] {
            {1,1,1},
            {1,-1,-1},
            {1,1,1},
        };
        mediumPatterns.Add(u);

        int[,] fountain = new int[3, 3] {
            {1,-1,-1},
            {1,1,1},
            {1,-1,-1},
        };
        mediumPatterns.Add(fountain);

        int[,] div2011 = new int[2, 2] {
            {12,1},
            {-1,1}
        };
        mediumPatterns.Add(div2011);

        int[,] bowl = new int[4, 3] {
            {-1,1,1 },
            {1,-1,1 },
            {1,-1,1 },
            {-1,1,1 }

        };
        mediumPatterns.Add(bowl);


        int[,] bird = new int[3, 4] {
            {-1,-1,-1,1 },
            {1,1,1,1 },
            {-1,1,1,-1 },

        };
        mediumPatterns.Add(bird);

        int[,] twist = new int[3, 4] {
            {1,-1,1,-1 },
            {-1,1,-1,1 },
            {1,-1,1,-1 },

        };
        mediumPatterns.Add(twist);

        int[,] hand = new int[3, 3] {
            {1,-1,-1},
            {1,-1,1},
            {1,1,-1},
        };
        mediumPatterns.Add(hand);


        int[,] sphinx = new int[4, 3] {
            {1,-1,-1 },
            {1,1,1 },
            {1,1,-1 },
            {1,1,-1 }

        };
        mediumPatterns.Add(sphinx);


        /*        { -1,-1,-1,-1 },
                    { -1,-1,-1,-1 },
                    { -1,-1,-1,-1 },
                    { -1,-1,-1,-1 }*/


        patterns.AddRange(easyPatterns);
        patterns.AddRange(mediumPatterns);
        patterns.AddRange(hardPatterns);

    }

    public int[,] getRandomPattern()
    {
        int max = patterns.Count;
        return patterns[UnityEngine.Random.Range(0, max)];
    }

    public int[,] getRandomEasyPattern()
    {
        int max = easyPatterns.Count;
        return easyPatterns[UnityEngine.Random.Range(0, max)];
    }

    public int[,] getRandomMediumPattern()
    {
        int max = mediumPatterns.Count;
        return mediumPatterns[UnityEngine.Random.Range(0, max)];
    }

    public int[,] getRandomHardPattern()
    {
        int max = hardPatterns.Count;
        return hardPatterns[UnityEngine.Random.Range(0, max)];
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

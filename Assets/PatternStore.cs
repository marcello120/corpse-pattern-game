using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternStore
{

    public List<int[,]> patterns;
    public List<int[,]> easyPatterns;
    public List<int[,]> mediumPatterns;
    public List<int[,]> hardPatterns;


    public PatternStore()
    {
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
        return patterns[Random.Range(0, max)];
    }

    public int[,] getRandomEasyPattern()
    {
        int max = easyPatterns.Count;
        return easyPatterns[Random.Range(0, max)];
    }

    public int[,] getRandomMediumPattern()
    {
        int max = mediumPatterns.Count;
        return mediumPatterns[Random.Range(0, max)];
    }

    public int[,] getRandomHardPattern()
    {
        int max = hardPatterns.Count;
        return hardPatterns[Random.Range(0, max)];
    }

}

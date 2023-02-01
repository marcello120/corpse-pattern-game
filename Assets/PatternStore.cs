using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternStore
{

    public List<int[,]> patterns;
    public List<int[,]> easyPatterns;
    public List<int[,]> hardPatterns;

    public PatternStore()
    {
        patterns = new List<int[,]>();
        easyPatterns = new List<int[,]>();
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
        hardPatterns.Add(wave);

        int[,] snake = new int[4, 2] {
            {1,1},
            {1,-1},
            {1,-1},
            {1,-1}
        };
        hardPatterns.Add(snake);

        int[,] reed = new int[2, 3] {
            {1,1,1},
            {1,-1,-1}
        };
        hardPatterns.Add(reed);

        int[,] eye = new int[3, 3] {
            {-1,1,-1},
            {1,-1,1},
            {-1,1,-1},
        };
        hardPatterns.Add(eye);


        patterns.AddRange(easyPatterns);
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

    public int[,] getRandomHardPattern()
    {
        int max = hardPatterns.Count;
        return hardPatterns[Random.Range(0, max)];
    }

}

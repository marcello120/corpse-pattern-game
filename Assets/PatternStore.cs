using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternStore
{

    public List<int[,]> patterns;

    public PatternStore()
    {
        patterns = new List<int[,]>();

        int[,] verticalLine = new int[1, 3] {
            {1,1,1}
        };
        patterns.Add(verticalLine);


        int[,] horizontalLine = new int[3, 1] {
            {1},
            {1},
            {1}
        };
        patterns.Add(horizontalLine);

        int[,] toprightcorner = new int[2, 2] {
            {1,1},
            {1,-1}
        };
        patterns.Add(toprightcorner);

        int[,] bottomrightcorner = new int[2, 2] {
            {1,1},
            {-1,1}
        };
        patterns.Add(bottomrightcorner);

        int[,] topleftcorner = new int[2, 2] {
            {1,-1},
            {1,1}
        };
        patterns.Add(topleftcorner);

        int[,] bottomleftcorner = new int[2, 2] {
            {-1,1},
            {1,1}
        };
        patterns.Add(bottomleftcorner);

        int[,] wave = new int[5, 2] {
            {-1,1},
            {1,-1},
            {-1,1},
            {1,-1},
            {-1,1}
        };
        patterns.Add(wave);

        int[,] snake = new int[4, 2] {
            {1,1},
            {1,-1},
            {1,-1},
            {1,-1}
        };
        patterns.Add(snake);

        int[,] reed = new int[2, 3] {
            {1,1,1},
            {1,-1,-1}
        };
        patterns.Add(reed);

        int[,] eye = new int[3, 3] {
            {-1,1,-1},
            {1,-1,1},
            {-1,1,-1},
        };
        patterns.Add(eye);

    }

    public int[,] getRandomPattern()
    {
        int max = patterns.Count;
        return patterns[Random.Range(0, max)];
    }

}

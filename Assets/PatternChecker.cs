using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternChecker 
{
    public PatternChecker()
    {

    }

    public bool checkForPattern(int[,] pattern, int[,] bigArr)
    {
        int windowWidth = pattern.GetLength(0);
        int windowHeight = pattern.GetLength(1);

        for (int i = 0; i < bigArr.GetLength(0) - windowWidth + 1; i++)
        {
            for (int j = 0; j < bigArr.GetLength(1) - windowHeight + 1; j++)
            {
                int[,] createdSubArr = createNewArrayWithOffset(windowWidth, windowHeight, i, j, bigArr);
                int[,] subsututedPatter = resolveWildCards(pattern, createdSubArr, -1);
                bool match = equalArrays(subsututedPatter, createdSubArr);
                if (match)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<Vector2Int> checkForPatternAndReturnPositions(int[,] pattern, int[,] bigArr)
    {
        int windowWidth = pattern.GetLength(0);
        int windowHeight = pattern.GetLength(1);

        for (int i = 0; i < bigArr.GetLength(0) - windowWidth + 1; i++)
        {
            for (int j = 0; j < bigArr.GetLength(1) - windowHeight + 1; j++)
            {
                int[,] createdSubArr = createNewArrayWithOffset(windowWidth, windowHeight, i, j, bigArr);
                int[,] subsututedPatter = resolveWildCards(pattern, createdSubArr, -1);
                bool match = equalArrays(subsututedPatter, createdSubArr);
                if (match)
                {
                    return Convert2DArrayToListOfVector2(i,j,createdSubArr, pattern);
                }
            }
        }

        return new List<Vector2Int>();
    }

    private List<Vector2Int> Convert2DArrayToListOfVector2(int xOffset, int yOffset, int[,] subArray, int[,] inPattern)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int i = 0; i < subArray.GetLength(0); i++)
        {
            for (int j = 0; j < subArray.GetLength(1); j++)
            {
                //1 is genric corpse. spec corpses are >10
                if (subArray[i, j] == inPattern[i, j] || (inPattern[i, j]==1 && subArray[i, j]> 10) || tierEqual(inPattern[i, j], subArray[i,j]))
                {
                    int xpos = i + xOffset;
                    int ypos = j + yOffset;
                    result.Add(new Vector2Int(xpos, ypos));
                }
            }
        }

        return result;

    }

    private int[,] createNewArray(int width, int height, int[,] source)
    {
        int[,] newArray = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                newArray[i, j] = source[i, j];
            }
        }

        return newArray;
    }

    private int[,] createNewArrayWithOffset(int width, int height, int widthOffSet, int heightOffset, int[,] source)
    {
        int[,] newArray = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                newArray[i, j] = source[i + widthOffSet, j + heightOffset];
            }
        }

        return newArray;
    }

    private bool equalArrays(int[,] array1, int[,] array2)
    {
        if (array1.GetLength(0) != array2.GetLength(0) || array1.GetLength(1) != array2.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < array1.GetLength(0); i++)
        {
            for (int j = 0; j < array1.GetLength(1); j++)
            {
                //1 means any corpse, coprses are >10
                if (array1[i, j] != array2[i, j] && (array1[i, j] != 1 || array2[i, j] <= 10) && !tierEqual(array1[i, j], array2[i,j]))
                {
                    return false;
                }
                if(array1[i, j] == 1 && array2[i, j] == 201)// do not hardcode this
                {
                    return false;
                }
            }
        }

        return true;

    }

    private bool tierEqual(int v1, int v2)
    {
        if (v1 % 10 == 0 && v2 > 10)
        {
            return v2/10 == v1/10;
        }
        return false;
    }

    private int[,] resolveWildCards(int[,] inPattern, int[,] bigArr, int wildcard)
   {
        int[,] returnArray = new int[inPattern.GetLength(0), inPattern.GetLength(1)];

        for (int i = 0; i < inPattern.GetLength(0); i++)
        {
            for (int j = 0; j < inPattern.GetLength(1); j++)
            {
                if (inPattern[i, j] == wildcard)
                {
                    returnArray[i, j] = bigArr[i, j];
                }
                else
                {
                    returnArray[i, j] = inPattern[i, j];
                }

            }
        }
        return returnArray;
    }
}

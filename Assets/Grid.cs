using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Grid 
{
    public int width;
    public int height;
    public float gridCellSize;
    public int defaultValue;

    private bool sliding = true;

    public int[,] array;

    public Grid( int width, int height, float gridCellSize, int defaultValue)
    {
        this.width = width;
        this.height = height;
        array = new int[width, height];
        this.gridCellSize = gridCellSize;
        this.defaultValue = defaultValue;
        InitGridWithAllInts(defaultValue);
    }

    public void InitGridWithAllInts(int value)
    {
        //populate array;
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                //CreateWorldText(gridArray[x, y].ToString(), null, getWorldPositionGrid(x, y), 1, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(getWorldPositionGridWithOffset(x, y), getWorldPositionGridWithOffset(x, y + 1), Color.black, 200f);
                Debug.DrawLine(getWorldPositionGridWithOffset(x, y), getWorldPositionGridWithOffset(x + 1, y), Color.black, 200f);
                //Instantiate(square, getWorldPositionGridWithOffset(x, y) + new Vector3(gridCellSize, gridCellSize) * 0.5f, Quaternion.identity);

                array[x, y] = 0;
            }
            Debug.DrawLine(getWorldPositionGridWithOffset(0, height), getWorldPositionGridWithOffset(width, height), Color.black, 200f);
            Debug.DrawLine(getWorldPositionGridWithOffset(width, 0), getWorldPositionGridWithOffset(width, height), Color.black, 200f);
        }
    }

    public static Vector3 adjustWoldPosToNearestCell(Vector3 worldPos, float gridCellSizeIn)
    {
        Vector3 pos = worldPos;
        float nearestX = findMultiple(pos.x, gridCellSizeIn);
        float nearestY = findMultiple(pos.y, gridCellSizeIn);
        Vector3 aprox = new Vector3(nearestX, nearestY, 0);

        float xDiff = aprox.x - pos.x;
        float yDiff = aprox.y - pos.y;

        if (xDiff < 0)
        {
            nearestX += gridCellSizeIn / 2;
        }
        else
        {
            nearestX -= gridCellSizeIn / 2;
        }

        if (yDiff < 0)
        {
            nearestY += gridCellSizeIn / 2;
        }
        else
        {
            nearestY -= gridCellSizeIn / 2;
        }

        Vector3 finalPos = new Vector3(nearestX, nearestY, 0);

        return finalPos;
    }

    private static float findMultiple(float value, float factor)
    {
        float nearestMultiple =
                (float)Math.Round(
                     (value / (float)factor),
                     MidpointRounding.AwayFromZero
                 ) * factor;

        return nearestMultiple;
    }

    public void addWorldPosToArray(Vector3 worldPos)
    {
        addWorldPosToArray(worldPos, 1);
    }


    public void addWorldPosToArray(Vector3 worldPos, int corpseNumber)
    {
        Vector3 arrayPos = ConvetWorldPosToArrayPos(worldPos);
        int xint = (int)Mathf.Round(arrayPos.x);
        int yint = (int)Mathf.Round(arrayPos.y);
        
        if(xint < 0 || xint > width || yint < 0 || yint > height)
        {
            Debug.LogError("OUTSIDE GRID CONFIDES! " + xint + " : " + yint);
            return;
        }

        array[xint, yint] = corpseNumber;
        Print();
    }
    public void RemoveFromArray(int x, int y)
    {
        array[x, y] = defaultValue;
    }

    public void SetField(int x, int y, int value)
    {
        array[x, y] = value;
    }

    public Vector3 getWorldPositionGridNoOffset(int x, int y)
    {
        return new Vector3(x, y) * gridCellSize;
    }

    public Vector3 getWorldPositionGridWithOffset(int x, int y)
    {
        return new Vector3(x, y) * gridCellSize + new Vector3(width / 2 * -gridCellSize, height / 2 * -gridCellSize);
    }
 
    public Vector3 ConvetWorldPosToArrayPos(Vector3 worldPos)
    {
        //get x from worldPos
        float wx = worldPos.x;
        //remove 0.8 to set at origo.
        wx -= gridCellSize/2;
        //devide by 0.16 and add offset of 11
        float localX = (wx / gridCellSize) + width / 2;

        //get x from worldPos
        float wy = worldPos.y;
        //remove 0.8 to set at origo.
        wy -= gridCellSize/2;
        //devide by 0.16 and add offset of 11
        float localY = (wy / gridCellSize) + height / 2;

        Vector3 result = new Vector3(localX, localY, 0);
        Debug.Log("Result: " + result);
        return result;
    }

        public void Print()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                sb.Append(array[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}

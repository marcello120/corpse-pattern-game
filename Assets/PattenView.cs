using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PattenView : MonoBehaviour
{
    /*    int[,] pattern = new int[3, 1] {
            {1},
            {1},
            {1}
        };*/
    int[,] pattern = new int[1, 3] {
        {1,1,1}
    };
    public GameObject square;

    public GameObject corpse;

    public Text ancor;


    public List<GameObject> corpses;

    public List<GameObject> squares;

    private float patternSize = 0.4f;

    public bool big;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetPattern(int[,] newPattern)
    {
        makeUnbig();
        transform.localScale = Vector3.one;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        pattern = newPattern;


        for (int i = 0; i < pattern.GetLength(0); i++)
        {
            for (int j = 0; j < pattern.GetLength(1); j++)
            {
                if (pattern[i, j] == 1)
                {
                    GameObject squareObj = Instantiate(square, transform.position + new Vector3(i, j) * patternSize - new Vector3(-1, pattern.GetLength(1) * patternSize), Quaternion.identity, gameObject.transform);
                    GameObject corpseObj = Instantiate(corpse, transform.position + new Vector3(i, j) * patternSize - new Vector3(-1, pattern.GetLength(1) * patternSize), Quaternion.identity, gameObject.transform);


                    /*
                                        GameObject squareObj = Instantiate(square, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.localPosition - new Vector3(0, patternGrid.gridCellSize*patternGrid.width), Quaternion.identity, gameObject.transform);
                                        GameObject corpseObj = Instantiate(corpse, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.localPosition, Quaternion.identity, gameObject.transform);

                    */
                    squares.Add(squareObj);
                    corpses.Add(corpseObj);
                }
            }
        }
        scaleView(newPattern);
        if (big)
        {
            makeBig();
        }

    }

    private void scaleView(int[,] newPattern)
    {
        if (newPattern.GetLength(0) > 3 || newPattern.GetLength(1) > 3)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1);
        }
        else
        {
            transform.localScale = Vector3.one;

        }
    }

    public void toggleBig()
    {
        if (!big)
        {
            makeBig();
            big = true;

        }
        else
        {
            makeUnbig();
            big = false;
        }

    }

    private void makeBig()
    {
        if (pattern.GetLength(0) > 3 || pattern.GetLength(1) > 3)
        {
            transform.localScale = new Vector3(2f, 2f, 1);
        }
        else
        {
            transform.localScale = new Vector3(3f, 3f, 1);

        }
        setOpacity(corpses, 0.4f);
        setOpacity(squares, 0.4f);


    }

    private void makeUnbig()
    {
        scaleView(pattern);
        setOpacity(corpses,1f);
        setOpacity(squares,1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void setOpacity(List<GameObject> list, float opacity)
    {
        foreach (GameObject obj in list)
        {
            if (obj == null)
            {
                continue;
            }
            // Get the Image component
            Image imageComponent = obj.GetComponent<Image>();

            // Check if the Image component exists
            if (imageComponent != null)
            {
                // Get the current color
                Color currentColor = imageComponent.color;

                // Set the transparency to 50%
                currentColor.a = opacity;

                // Assign the new color with adjusted transparency
                imageComponent.color = currentColor;
            }
            else
            {
                Debug.LogWarning("Image component not found in " + obj.name);
            }
        }
    }

}

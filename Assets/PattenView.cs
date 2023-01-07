using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PattenView : MonoBehaviour
{
    int[,] pattern = new int[3, 3] {
        {-1,-1,-1},
        {1,1,1},
        {-1,-1,-1}
    };
    private Grid patternGrid;

    public GameObject square;

    public GameObject corpse;

    public List<GameObject> corpses;

    public List<GameObject> squares;

    // Start is called before the first frame update
    void Start()
    {
        corpses= new List<GameObject>();
        squares= new List<GameObject>();
    }

    public void SetPattern(int[,] newPattern)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        pattern = newPattern;
        patternGrid = new Grid(pattern.GetLength(0), pattern.GetLength(1), 0.16f, 0);
        patternGrid.InitGridWithAllInts(0);

        for (int i = 0; i < pattern.GetLength(0); i++)
        {
            for (int j = 0; j < pattern.GetLength(1); j++)
            {
                if (pattern[i, j] == 1)
                {
                    GameObject squareObj = Instantiate(square, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.position, Quaternion.identity, gameObject.transform);
                    GameObject corpseObj = Instantiate(corpse, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.position, Quaternion.identity, gameObject.transform);

                    squares.Add(squareObj);
                    corpses.Add(corpseObj);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}

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


        for (int i = 0; i < pattern.GetLength(0); i++)
        {
            for (int j = 0; j < pattern.GetLength(1); j++)
            {
                if (pattern[i, j] == 1)
                {
                    GameObject squareObj = Instantiate(square, transform.position + new Vector3(i *1.15f, j*1.15f) - new Vector3(-1,pattern.GetLength(1)), Quaternion.identity, gameObject.transform);
                    GameObject corpseObj = Instantiate(corpse, transform.position + new Vector3(i * 1.15f, j * 1.15f) - new Vector3(-1, pattern.GetLength(1)), Quaternion.identity, gameObject.transform);


                    /*
                                        GameObject squareObj = Instantiate(square, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.localPosition - new Vector3(0, patternGrid.gridCellSize*patternGrid.width), Quaternion.identity, gameObject.transform);
                                        GameObject corpseObj = Instantiate(corpse, patternGrid.getWorldPositionGridNoOffset(i, j) + new Vector3(patternGrid.gridCellSize, patternGrid.gridCellSize) * 0.5f + transform.localPosition, Quaternion.identity, gameObject.transform);

                    */
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

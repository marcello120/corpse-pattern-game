
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text successText;

    public GameObject square;

    public GameObject corpseCleaner;

    private PatternChecker patternChecker;

    private PatternStore patternStore;

    public PattenView pattenView;

    public AudioSource scoreSound;

    public Enemy slime;

    public int width = 22;
    public int height = 12;
    public float gridCellSize = 0.16f;

    public Grid grid;

    public bool success = false;

    int[,] pattern;

    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(width/2 * -gridCellSize, height/2 * -gridCellSize);

        patternChecker = new PatternChecker();
        patternStore = new PatternStore();

        //create array
        grid = new Grid(width,height,gridCellSize,0);

        //set pattern
        pattern = patternStore.getRandomEasyPattern();
        pattenView.SetPattern(pattern);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 AddWorldPosToGridAndReturnAdjustedPos(Vector3 worldPos)
    {
        return AddWorldPosToGridAndReturnAdjustedPos(worldPos, 1);
    }


    public Vector3 AddWorldPosToGridAndReturnAdjustedPos(Vector3 worldPos, int corpsenumber)
    {
        //spawn replacement slime
        SpawnSlime();

        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = grid.adjustWoldPosToNearestCell(worldPos);

        //add adjusted world position to grid
        grid.addWorldPosToArray(adjustedPos, corpsenumber);

        //check if pattern is found in grid
        List<Vector2Int> fitPatter = patternChecker.checkForPatternAndReturnPositions(pattern, grid.array);

        //if pattern is found - remove objects and move game along
        if (fitPatter.Count > 0)
        {
            for (int i = 0; i < fitPatter.Count; i++)
            {
                //SCORE!!!!
                scoreSound.Play();

                //Instantiate(square, grid.getWorldPositionGridWithOffset(fitPatter[i].x, fitPatter[i].y) + new Vector3(grid.gridCellSize, grid.gridCellSize) * 0.5f, Quaternion.identity);

                //add a corpse cleaner, with collides with corpse then removes corpese and self
                Instantiate(corpseCleaner, grid.getWorldPositionGridWithOffset(fitPatter[i].x, fitPatter[i].y) + new Vector3(grid.gridCellSize, grid.gridCellSize) * 0.5f, Quaternion.identity);
                //also clear corpse from array, use new removeCorpse method
                grid.RemoveFromArray(fitPatter[i].x, fitPatter[i].y);

            }
            success = true;
            Debug.Log("SUCCESS");
           
            //get new random pattern from store
            pattern = patternStore.getRandomEasyPattern();

            //set new pattern on UI
            pattenView.SetPattern(pattern);

            //Increment score and set UI
            score = score + 1;
            successText.text = score.ToString();
        }

        //return the world position where the enemy should place the coprse
        return adjustedPos;
    }

    private void SpawnSlime()
    {
        Vector2 vector2 = UnityEngine.Random.insideUnitCircle.normalized;
        Instantiate(slime,vector2,Quaternion.identity);
    }
}

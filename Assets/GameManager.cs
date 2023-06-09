
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI successText;

    public TextMeshProUGUI hightText;

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

    public int highscore = 0;

    public GameObject spawnerParent;

    public float enemyCount = 2;

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
        highscore = PlayerPrefs.GetInt("HighScore", 0);

        successText.SetText("Score: " + 0);
        hightText.SetText("Top:  " + highscore);

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
       SpawnSlime(worldPos);

        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = Grid.adjustWoldPosToNearestCell(worldPos,grid.gridCellSize);

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
            pattern = patternStore.getRandomMediumPattern();

            //set new pattern on UI
            pattenView.SetPattern(pattern);

            //Increment score and set UI
            incrementScore(); 
           
        }

        //return the world position where the enemy should place the coprse
        return adjustedPos;
    }

    public void incrementScore()
    {
        score = score + 1;
        successText.SetText("Score: " + score);
        if(score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("HighScore", highscore);
            hightText.SetText("Top:  " + highscore);
        }
    }

    private void SpawnSlime(Vector3 pos )
    {
        List<EnemySpawner> spawners = spawnerParent.GetComponentsInChildren<EnemySpawner>().OfType<EnemySpawner>().ToList();

        List<EnemySpawner> sortedSpawners = spawners.OrderByDescending(t => Vector3.Distance(pos, t.transform.position)).ToList();

        if(sortedSpawners.Count > 0 && sortedSpawners[0]!=null) {
            sortedSpawners[0].spawnEnemy();
        }

        if ((score > 10 && enemyCount == 3) || (score > 5 && enemyCount == 2))
        {
            if (sortedSpawners.Count > 1 && sortedSpawners[1] != null)
            {
                sortedSpawners[1].spawnEnemy();
                enemyCount++;
            }
        }
     
    }
}

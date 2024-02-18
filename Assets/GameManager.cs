
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI successText;
    public TextMeshProUGUI successDeathText;

    public TextMeshProUGUI hightText;

    public GameObject corpseCleaner;

    public GameObject rock;

    private PatternChecker patternChecker;

    private PatternStore patternStore;

    public PattenView pattenView;

    public AudioSource scoreSound;

    public int width = 22;
    public int height = 12;
    public float gridCellSize = 0.5f;

    public Grid grid;

    public Grid obstacles;

    public bool success = false;

    int[,] pattern;

    public int score = 0;

    public int highscore = 0;

    public GameObject spawnerParent;

    public float enemyCount = 2;
    public float currentEnemyCount = 1;


    public Doubler doublerPrefab;

    public Doubler doubler;

    public List<DoublerSpawner> doublerSpawners;

    public GameObject doublerSpawnerParent;

    public RiggedPlayerController player;

    public PowerUpSelection powerUpSelection;

    public static GameManager Instance;



    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        transform.position = new Vector3(width / 2 * -gridCellSize, height / 2 * -gridCellSize);

        patternChecker = new PatternChecker();
        patternStore = new PatternStore();

        //create array
        grid = new Grid(width, height, gridCellSize, 0);

        obstacles = new Grid(width, height, gridCellSize, 0);

        //set pattern
        pattern = patternStore.getRandomEasyPattern();
        pattenView.SetPattern(pattern);
        highscore = PlayerPrefs.GetInt("HighScore", 0);

        successText.SetText("Score: " + 0);
        hightText.SetText("Top:  " + highscore);


        doublerSpawners = doublerSpawnerParent.GetComponentsInChildren<DoublerSpawner>().OfType<DoublerSpawner>().ToList();


        player = (RiggedPlayerController)GameObject.FindFirstObjectByType(typeof(RiggedPlayerController));

        SpawnDoubler();

        InvokeRepeating(nameof(SpawnEnemies), 0f, 7.5f);

        //instead of count keep list
        currentEnemyCount = GameObject.FindObjectsOfType<Enemy>().Sum(item => item.powerLevel);



    }

    void SpawnEnemies()
    {
        while(enemyCount > currentEnemyCount)
        {
            Debug.Log("SPAWNING ENEMY");
            SpawnSlime((int)(enemyCount-currentEnemyCount));
        }
    }

    private void SpawnDoubler()
    {
        //DoublerSpawner selected = doublerSpawners[UnityEngine.Random.Range(0, doublerSpawners.Count)];

        doubler = SpawnWithCheck(doublerPrefab.gameObject,player.transform.position,8,10).GetComponent<Doubler>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 AddWorldPosToGridAndReturnAdjustedPos(Vector3 worldPos, int corpsenumber, int powerLevel)
    {
        currentEnemyCount-=powerLevel;


        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = Grid.adjustWoldPosToNearestCell(worldPos, grid.gridCellSize);

        //add adjusted world position to grid
        grid.addWorldPosToArray(adjustedPos, corpsenumber);

        //check if pattern is found in grid
        List<Vector2Int> fitPatter = patternChecker.checkForPatternAndReturnPositions(pattern, grid.array);


        //if pattern is found - remove objects and move game along
        if (fitPatter.Count > 0)
        {
            int multiplier = 1;
            Vector3 doublerLocAdjusted = Grid.adjustWoldPosToNearestCell(doubler.transform.position, grid.gridCellSize);
            Vector3 doublerLoc = grid.ConvetWorldPosToArrayPos(doublerLocAdjusted);


            for (int i = 0; i < fitPatter.Count; i++)
            {
                if (fitPatter[i].x == doublerLoc.x && fitPatter[i].y == doublerLoc.y)
                {
                    //doubler is part of pattern
                    multiplier = 2;
                    Debug.Log("Multiplier detected");
                    Destroy(doubler);
                    SpawnDoubler();
                }

                //SCORE!!!!
                scoreSound.Play();

                Vector2Int corpseLoc = fitPatter[i];
                removeCorpseAtGridLoc(corpseLoc);

            }
            success = true;
            Debug.Log("SUCCESS");

            //get new random pattern from store
            pattern = patternStore.getRandomEasyPattern();

            //set new pattern on UI
            pattenView.SetPattern(pattern);

            //Increment score and set UI
            incrementScore(multiplier);

        }

        //return the world position where the enemy should place the coprse
        return adjustedPos;
    }


    public void removeCorpseAtWorldPos(Vector3 pos)
    {
        Vector3 corpseLocAdj = Grid.adjustWoldPosToNearestCell(pos, grid.gridCellSize);
        Vector3 coprseLoc = grid.ConvetWorldPosToArrayPos(corpseLocAdj);
        removeCorpseAtGridLoc(new Vector2Int(Mathf.FloorToInt(coprseLoc.x), Mathf.FloorToInt(coprseLoc.y)));
    }

    private void removeCorpseAtGridLoc(Vector2Int corpseLoc)
    {
        //add a corpse cleaner, with collides with corpse then removes corpese and self
        Instantiate(corpseCleaner, grid.getWorldPositionGridWithOffset(corpseLoc.x, corpseLoc.y) + new Vector3(grid.gridCellSize, grid.gridCellSize) * 0.5f, Quaternion.identity);
        //also clear corpse from array, use new removeCorpse method
        grid.RemoveFromArray(corpseLoc.x, corpseLoc.y);
    }

    public void incrementScore(int multi)
    {
        score = score + 1 * multi;
        successText.SetText("Score: " + score);
        successDeathText.SetText(score.ToString());
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("HighScore", highscore);
            hightText.SetText("Top:  " + highscore);
        }
        if ((player.level * 1) - 1 < score && score != 0)
        {
            StartCoroutine(levelUp());
            player.levelUp();
        }
        if (score / 5 + 1 >= enemyCount)
        {
            enemyCount++;
        }
    }

    private IEnumerator levelUp()
    {
        yield return new WaitForSeconds(0.75f);

        powerUpSelection.show();
        Time.timeScale = 0;
        Debug.Log("Level Up");
    }

    public void SpawnSlime(int maxPower)
    {
        //List<EnemySpawner> spawners = spawnerParent.GetComponentsInChildren<EnemySpawner>().OfType<EnemySpawner>().ToList();

        //List<EnemySpawner> sortedSpawners = spawners.OrderByDescending(t => Vector3.Distance(pos, t.transform.position)).ToList();

        Enemy enemy = EnemyStore.instance.getRandomEnemyWithMaxPower(maxPower);
        currentEnemyCount += enemy.powerLevel;

        Debug.Log("SPAWNING " + enemy.name);


        SpawnWithCheck(enemy.gameObject, player.transform.position, 8, 10);
    }

    private Vector2Int worldPosToGridPos(Vector3 pos, Grid inputGrid)
    {
        Vector3 intialLocation = Grid.adjustWoldPosToNearestCell(pos, inputGrid.gridCellSize);
        Vector3 initialGridPos = inputGrid.ConvetWorldPosToArrayPos(intialLocation);
        int xint = (int)Mathf.Round(initialGridPos.x);
        int yint = (int)Mathf.Round(initialGridPos.y);
        return new Vector2Int(xint, yint);
    }
    private Vector2Int getRandomGridPointInRadius(Vector2Int pos, int innerX, int outerX)
    {
        int xint = pos.x;
        int yint = pos.y;

        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f); // Random angle in radians
        float radius = UnityEngine.Random.Range(innerX, outerX); // Random radius between x1 and x2

        // Calculate the random position using polar coordinates
        int x = Mathf.RoundToInt(radius * Mathf.Cos(angle));
        int y = Mathf.RoundToInt(radius * Mathf.Sin(angle));

        int xintAdj = xint + x;
        int yintAdj = yint + y;

        Debug.Log("RANDO " + xintAdj + " " + yintAdj);


        if (xintAdj > width - 1 || xintAdj <= 1)
        {
            xintAdj = xint - x;
        }
        if (yintAdj > height - 1 || yintAdj <= 1)

        {
            yintAdj = yint - y;
        }

        if ((xintAdj > width - 1 || xintAdj <= 1 || yintAdj > width - 1 || yintAdj <= 1))
        {
            Debug.Log(" We fucked up.");
            return new Vector2Int(0, 0);
        }

        return (new Vector2Int(xintAdj, yintAdj));

    }

    //999 if not good
    private Vector3 trySpawnInGrid(Vector2Int pos, Grid inGrid)
    {
        int arrayWidth = inGrid.array.GetLength(0);
        int arrayHeight = inGrid.array.GetLength(1);

        if (!IsWithinBounds(pos, arrayWidth, arrayHeight))
        {
            return new Vector3(999, 999);
        }

        int valueAtPoint = inGrid.array[pos.x, pos.y];

        if (valueAtPoint != 0)
        {
            return new Vector3(999, 999);
        }

        Vector3 worldPos = inGrid.getWorldPositionGridWithOffset(pos.x, pos.y) + new Vector3(gridCellSize / 2, gridCellSize / 2);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPos, 0.2f);

        if (hitColliders.Length > 0)
        {
            inGrid.SetField(pos.x, pos.y, 101);
            return new Vector3(999, 999);
        }

        return worldPos;
    }

    private bool IsWithinBounds(Vector2Int pos, int width, int height)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    //to spawn anywhere in grid set spawnWorldPos to 0,0 and outer x/y to width/height and inner to 0
    //to spawn in given place set variance to 0
    //0,0 needs to be a safe space to spawn as fallback
    public GameObject SpawnWithCheck(GameObject thingToSpawn, Vector3 spawnWorldPos, int innerX, int outerX)
    {
        Debug.Log("Spawining " + thingToSpawn.name + " at " + spawnWorldPos + " with inner of " + innerX + " and outer " + outerX);

        Vector3 spawnPoint = getSpawnPoint(spawnWorldPos, innerX, outerX);

        return Instantiate(thingToSpawn, spawnPoint, Quaternion.identity);

    }

    public Vector3 getSpawnPoint(Vector3 spawnWorldPos, int innerX, int outerX)
    {
        Vector2Int gridPos = worldPosToGridPos(spawnWorldPos, obstacles);

        int tryCount = 0;
        int maxTries = 200;
        bool success = false;

        while (tryCount < maxTries && !success)
        {
            Vector2Int randomPos = getRandomGridPointInRadius(gridPos, innerX, outerX);
            Vector3 spawnCandidate = trySpawnInGrid(randomPos, obstacles);

            if (spawnCandidate.x == 999)
            {
                tryCount++;
            }
            else
            {
                return spawnCandidate;
            }

        }
        return new Vector3(0, 0, 0);
    }

}

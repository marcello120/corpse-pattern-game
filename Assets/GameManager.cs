
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Level currentLevel;

    public GameObject boss;

    public bool bossmode = false;

    public TextMeshProUGUI successText;
    public TextMeshProUGUI successDeathText;

    public TextMeshProUGUI deathScore;
    public TextMeshProUGUI deathExtraCorpses;
    public TextMeshProUGUI deathTimeBonus;


    public TextMeshProUGUI hightText;

    public GameObject corpseCleaner;

    public GameObject rock;

    private PatternChecker patternChecker;

    private PatternStore patternStore;

    //public PattenView pattenView;

    public PatternGrid patternGrid;

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

    public float gameTime = 0;

    public GameObject corpseMoundObj;

    public GameObject scoreEffect;

    public List<PatternStore.CorpsePattern> patternList;

    public GameObject snakeBoss;

    public WinMenu winMenu;

    public enum Level
    {
        ENDLESS,
        LEVEL1_1,
        LEVEL1_2,
        LEVEL1_3
    }

    public class LevelCongfig
    {
        public List<PatternStore.CorpsePattern.Difficulty> difficulties;
        public int spiceChance;
        public bool endsWithBoss;

        public LevelCongfig(List<PatternStore.CorpsePattern.Difficulty> difficulties, int spiceChance, bool endsWithBoss)
        {
            this.difficulties = difficulties;
            this.spiceChance = spiceChance;
            this.endsWithBoss = endsWithBoss;
        }
    }

    public Dictionary<Level, LevelCongfig> LevelConfigs = new Dictionary<Level, LevelCongfig>()
{
            {
        Level.ENDLESS, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {
                
            },
            spiceChance: 20,
            endsWithBoss: false
        )
    },
    {
        Level.LEVEL1_1, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM
            },
            spiceChance: 100,  
            endsWithBoss: false 
        )
    },
    {
        Level.LEVEL1_2, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
            },
            spiceChance: 30,  
            endsWithBoss: false 
        )
    },
    {
        Level.LEVEL1_3, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.HARD,
            },
            spiceChance: 10,  
            endsWithBoss: true 
        )
    },
};



    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        transform.position = new Vector3(width / 2 * -gridCellSize, height / 2 * -gridCellSize);

        patternChecker = new PatternChecker();
        patternStore = PatternStore.Instance;

        //create array
        grid = new Grid(width, height, gridCellSize, 0);

        obstacles = new Grid(width, height, gridCellSize, 0);


        if (currentLevel == Level.ENDLESS)
        {
            PatternStore.CorpsePattern corpsePattern = patternStore.GetPatternByName("bottom right corner");
            pattern = corpsePattern.getPatternFrom2DArray();

        }
        else
        {
            List<PatternStore.CorpsePattern.Difficulty> diffs = LevelConfigs[currentLevel].difficulties;
            for (int i = 0; i < diffs.Count; i++)
            {
                patternList.Add(patternStore.GetRandomPatternWithDifficulty(diffs[i]));
            }
            pattern = patternList[0].getPatternFrom2DArray();

            patternList.RemoveAt(0);
        }
        pattern = CorpseStore.Instance.spiceItUp(pattern, LevelConfigs[currentLevel].spiceChance);
        patternGrid.setPattern(pattern);
        highscore = PlayerPrefs.GetInt("HighScore_" + currentLevel.ToString(), 0);

        successText.SetText("Score: " + 0);
        hightText.SetText("Top:  " + highscore);


        doublerSpawners = doublerSpawnerParent.GetComponentsInChildren<DoublerSpawner>().OfType<DoublerSpawner>().ToList();

        player = (RiggedPlayerController)FindFirstObjectByType(typeof(RiggedPlayerController));

        SpawnDoubler();

        InvokeRepeating(nameof(SpawnEnemies), 0f, 7.5f);

        //instead of count keep list
        currentEnemyCount = GameObject.FindObjectsOfType<Enemy>().Sum(item => item.powerLevel);

        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    private int calculateScore()
    {
        return score * 10 - getNumberOfCorpsesOnGrind() + getTimeBonus() + (int)player.playerHealth; ;
    }

    private int getTimeBonus()
    {
        return (int)(score / gameTime * 100);
    }

    private int getNumberOfCorpsesOnGrind()
    {
        int numberOfCorpses = 0;
        for (int i = 0; i < grid.array.GetLength(0); i++)
        {
            for (int j = 0; j < grid.array.GetLength(1); j++)
            {
                if (grid.array[i, j] != 0)
                {
                    numberOfCorpses++;
                }
            }
        }
        return numberOfCorpses;
    }

    void SpawnEnemies()
    {
        if(currentLevel == Level.ENDLESS)
        {
            while (enemyCount > currentEnemyCount)
            {
                Debug.Log("SPAWNING ENEMY");
                SpawnSlime((int)(enemyCount - currentEnemyCount));
            }
        }else if (!bossmode)
        {
            while (enemyCount > currentEnemyCount)
            {
                Debug.Log("SPAWNING ENEMY");
                SpawnSlime((int)(enemyCount - currentEnemyCount));
            }
        }

    }

    private void SpawnDoubler()
    {
        //DoublerSpawner selected = doublerSpawners[UnityEngine.Random.Range(0, doublerSpawners.Count)];

        doubler = SpawnWithCheck(doublerPrefab.gameObject, player.transform.position, 8, 10).GetComponent<Doubler>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameTime += Time.fixedDeltaTime;
    }

    public IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        int finalScore = calculateScore();
        winMenu.gameObject.SetActive(true);
        winMenu.refresh(score, getNumberOfCorpsesOnGrind(), getTimeBonus(), (int)player.playerHealth, finalScore) ;
        PlayerPrefs.SetInt("HighScore_" + currentLevel.ToString(), finalScore);
        yield return new WaitForSeconds(0.1f);

    }

    public void Lose()
    {
        int finalScore = calculateScore();
        successDeathText.SetText(finalScore.ToString());
        deathScore.SetText((score * 10).ToString());
        deathExtraCorpses.SetText(getNumberOfCorpsesOnGrind().ToString());
        deathTimeBonus.SetText(getTimeBonus().ToString());
        PlayerPrefs.SetInt("HighScore_" + currentLevel.ToString(), finalScore);

    }

    public int removeCoprseAndReturnID(Vector2Int gridPos)
    {
        int coprpseNumberToReturn = grid.array[gridPos.x, gridPos.y];

        grid.array[gridPos.x, gridPos.y] = 0;


        return coprpseNumberToReturn;
    }

    public class CoprseInfoObject
    {
        public int coprseNumber;
        public Vector3 corpseWorldPos;
        public GameObject corpseMound;

        public CoprseInfoObject(int coprseNumber, Vector3 corpseWorldPos, GameObject corpseMound)
        {
            this.coprseNumber = coprseNumber;
            this.corpseWorldPos = corpseWorldPos;
            this.corpseMound = corpseMound;
        }
    }


    public CoprseInfoObject AddWorldPosToGridAndReturnAdjustedPos(Vector3 worldPos, int corpsenumber, int powerLevel)
    {
        currentEnemyCount-=powerLevel;
        if (currentEnemyCount < 0) currentEnemyCount= 0;


        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = Grid.adjustWoldPosToNearestCell(worldPos, grid.gridCellSize);

        //add adjusted world position to grid
        int newCorpseNum =  grid.addWorldPosToArray(adjustedPos, corpsenumber);

        //if (newCorpseNum == 99)
        //{
        //    removeCorpseAtWorldPos(adjustedPos);
        //}
        if (newCorpseNum == 999)
        {
            removeCorpseAtWorldPos(adjustedPos);
            return new CoprseInfoObject(newCorpseNum, adjustedPos,corpseMoundObj);
        }

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
            Vector2Int middle = fitPatter[(fitPatter.Count-1) / 2];
            float size = pattern.GetLength(0) > pattern.GetLength(1) ? pattern.GetLength(0) : pattern.GetLength(1);
            GameObject effect =Instantiate(scoreEffect, grid.getWorldPositionGridWithOffset(middle.x, middle.y) + new Vector3(gridCellSize / 2, gridCellSize / 2), Quaternion.identity);
            effect.transform.localScale = effect.transform.localScale * size;
            //get new random pattern from store

            //Increment score and set UI
            incrementScore(multiplier);

            if (currentLevel != Level.ENDLESS)
            {
                if (patternList.Count() < 1)
                {
                    if (LevelConfigs[currentLevel].endsWithBoss)
                    {
                        if (!bossmode)
                        {
                            //spawn snakeboss
                            boss = Instantiate(snakeBoss.gameObject, Vector3.zero, Quaternion.identity);
                            bossmode = true;
                        }
                        else
                        {
                            if (corpsenumber == 111 && GameObject.FindObjectsOfType(typeof(SnakeBoss)).Count() == 1)
                            {
                                //killed last snake. Level Complete
                                StartCoroutine(Win());
                            }
                        }
                        pattern = (new int[1, 1] { { 111 } });
                    }
                    else
                    {
                        StartCoroutine(Win());
                    }
                }
                else
                {
                    pattern = patternList[0].getPatternFrom2DArray();
                    patternGrid.setPattern(patternList[0].getOtherPatternFrom2DArray());
                    patternList.RemoveAt(0);
                }
            }
            if (currentLevel == Level.ENDLESS)
            {
                PatternStore.CorpsePattern corpsePattern = patternStore.GetRandomPatternWithDifficulty(PatternStore.CorpsePattern.Difficulty.EASY);
                pattern = corpsePattern.getPatternFrom2DArray();

            }            
            
            pattern = CorpseStore.Instance.spiceItUp(pattern, LevelConfigs[currentLevel].spiceChance);
            patternGrid.setPattern(pattern);



        }

        //return the world position where the enemy should place the coprse
        return new CoprseInfoObject(newCorpseNum, adjustedPos,null);
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
        Debug.Log("SCORE:  " + calculateScore());
        successText.SetText("Score: " + score * 10);
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("HighScore_" + currentLevel.ToString(), highscore);
            hightText.SetText("Top:  " + highscore);
        }
        if ((player.level * 1) - 1 < score && score != 0 && (currentLevel == Level.ENDLESS || patternList.Count > 0) )
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

    public HashSet<Enemy> getEnemiesFromPatter(int[,] patternIn)
    {
        return patternIn.Cast<int>().ToHashSet().Select(code => EnemyStore.instance.getEnemyByCorpseCode(code)).ToHashSet();
    }

    public void SpawnSlime(int maxPower)
    {
        //List<EnemySpawner> spawners = spawnerParent.GetComponentsInChildren<EnemySpawner>().OfType<EnemySpawner>().ToList();

        //List<EnemySpawner> sortedSpawners = spawners.OrderByDescending(t => Vector3.Distance(pos, t.transform.position)).ToList();
        
        Enemy enemyToSpawn;


        //get enemies codes from pattern
        List<int> enemiesFromPattern = pattern.Cast<int>().ToList();
        //get enemies codes from alive enemies
        List<int> aliveCodes = GameObject.FindObjectsOfType<Enemy>().Select(a => a.corpseNumber).ToList();
        //get codes for corpses on ground
        HashSet<int> corpseCodes = grid.array.Cast<int>().ToHashSet();
        //remove alive/corpese from pattern
        List<int> filtered = enemiesFromPattern.Where(code => code %10 != 0 && code < 90 && code > 9).ToList();
        foreach (int item in aliveCodes)
        {
            // Remove the first occurrence of the item in list1 (if it exists)
            filtered.Remove(item);
        }
        if( UnityEngine.Random.Range(1, 10) <= 6)
        {
            foreach (int item in corpseCodes)
            {
                // Remove the first occurrence of the item in list1 (if it exists)
                filtered.Remove(item);
            }
        }
        //if not empty spawn in pattern
        if(filtered.Count > 0)
        {
             enemyToSpawn = EnemyStore.instance.getEnemyByCorpseCode(filtered.ToArray()[0]);
        }
        else
        {
             enemyToSpawn = EnemyStore.instance.getRandomEnemyWithMaxPower(maxPower);
        }

        currentEnemyCount += enemyToSpawn.powerLevel;

        Debug.Log("SPAWNING " + enemyToSpawn.name);

        if(player == null)
        {
            return;
        }
        SpawnWithCheck(enemyToSpawn.gameObject, player.transform.position, 8, 10);
    }

    public Vector2Int worldPosToGridPos(Vector3 pos, Grid inputGrid)
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPos, 0.2f, LayerMask.GetMask("Enemy", "ObstacleBlock","Player"));

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
        int maxTries = 100;
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

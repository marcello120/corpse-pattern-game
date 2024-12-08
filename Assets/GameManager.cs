
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

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

    public AudioSource backgroundMusic;


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

    public GameObject turtleBoss;

    public GameObject bombBoss;



    public WinMenu winMenu;

    public enum Level
    {
        HUB,
        ENDLESS,
        LEVEL1_1,
        LEVEL1_2,
        LEVEL1_3
    }

    public class LevelCongfig
    {
        public List<PatternStore.CorpsePattern.Difficulty> difficulties;
        public int spiceChance;
        public Type bossType;
        public int[,] bossPattern;

        public LevelCongfig(List<PatternStore.CorpsePattern.Difficulty> difficulties, int spiceChance, Type bossType, int[,] bossPattern)
        {
            this.difficulties = difficulties;
            this.spiceChance = spiceChance;
            this.bossType = bossType;
            this.bossPattern = bossPattern;
        }
    }

    public Dictionary<Level, LevelCongfig> levelConfigs = new Dictionary<Level, LevelCongfig>()
{
                    {
        Level.HUB, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {

            },
            spiceChance: 0,
            bossType: null,
            bossPattern: null
        )
    },
            {
        Level.ENDLESS, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {

            },
            spiceChance: 20,
            bossType: null,
            bossPattern: null
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
            },
            spiceChance: 100,
            bossType: typeof(TurtleBoss),
            bossPattern: new int[1, 2]{{ 142, 201 } }
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
            bossType: typeof(TheBombBoss),
            bossPattern:  new int[3, 1]
            {
                {11},
                {143},
                {11}
            }
        )
    },
    {
        Level.LEVEL1_3, new LevelCongfig(
            new List<PatternStore.CorpsePattern.Difficulty>
            {
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.EASY,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.MEDIUM,
                PatternStore.CorpsePattern.Difficulty.HARD,
            },
            spiceChance: 10,
            bossType: typeof(SnakeBoss),
            bossPattern: new int[1, 1] { { 111 } }
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

        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            setBackgroundMusicVolume(1f);
        }
        else
        {
            setBackgroundMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        }

        transform.position = new Vector3(width / 2 * -gridCellSize, height / 2 * -gridCellSize);

        patternChecker = new PatternChecker();
        patternStore = PatternStore.Instance;

        //create array
        grid = new Grid(width, height, gridCellSize, 0);

        obstacles = new Grid(width, height, gridCellSize, 0);

        player = (RiggedPlayerController)FindFirstObjectByType(typeof(RiggedPlayerController));


        if (currentLevel == Level.HUB)
        {

            pattern = new int[1, 1] { { 404 } };
        }
        else if (currentLevel == Level.ENDLESS)
        {
            PatternStore.CorpsePattern corpsePattern = patternStore.GetPatternByName("bottom right corner");
            pattern = corpsePattern.getPatternFrom2DArray();

        }
        else
        {
            List<PatternStore.CorpsePattern.Difficulty> diffs = levelConfigs[currentLevel].difficulties;
            for (int i = 0; i < diffs.Count; i++)
            {
                patternList.Add(patternStore.GetRandomPatternWithDifficulty(diffs[i]));
            }
            pattern = patternList[0].getPatternFrom2DArray();

            patternList.RemoveAt(0);
        }
        pattern = CorpseStore.Instance.spiceItUp(pattern, levelConfigs[currentLevel].spiceChance);
        patternGrid.setPattern(pattern);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        if (currentLevel == Level.HUB)
        {
            //set total score
            setTotal();
            return;
        }

        highscore = PlayerPrefs.GetInt("HighScore_" + currentLevel.ToString(), 0);

        successText.SetText("Score: " + 0);
        hightText.SetText("Top:  " + highscore);

     
        SpawnDoubler();

        InvokeRepeating(nameof(SpawnEnemies), 0f, 7.5f);

        //instead of count keep list
        currentEnemyCount = GameObject.FindObjectsOfType<Enemy>().Sum(item => item.powerLevel);

 
    }

    private static void setTotal()
    {
        GameObject totalScore = GameObject.Find("TotalScore");
        int totalScoreSaved = PlayerPrefs.GetInt("TotalScore", 0);
        totalScore.GetComponent<TextMeshProUGUI>().SetText("Total:" + totalScoreSaved);
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
        if (currentLevel == Level.ENDLESS)
        {
            while (enemyCount > currentEnemyCount)
            {
                Debug.Log("SPAWNING ENEMY");
                SpawnSlime((int)(enemyCount - currentEnemyCount));
            }
        }
        else if (!bossmode)
        {
            while (enemyCount > currentEnemyCount)
            {
                Debug.Log("SPAWNING ENEMY");
                SpawnSlime((int)(enemyCount - currentEnemyCount));
            }
        }else if (bossmode && currentLevel == Level.LEVEL1_2)
        {
            while (3 > currentEnemyCount)
            {
                Enemy enemyToSpawn = EnemyStore.instance.getEnemyByCorpseCode(11);

                currentEnemyCount += enemyToSpawn.powerLevel;

                SpawnWithCheck(enemyToSpawn.gameObject, player.transform.position, 8, 10);

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
        winMenu.refresh(score, getNumberOfCorpsesOnGrind(), getTimeBonus(), (int)player.playerHealth, finalScore);
        PlayerPrefs.SetInt("HighScore_" + currentLevel.ToString(), finalScore);
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore + finalScore);
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
        int totalScore = PlayerPrefs.GetInt("TotalScore", 0);
        PlayerPrefs.SetInt("TotalScore", totalScore + finalScore);

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
        currentEnemyCount -= powerLevel;
        if (currentEnemyCount < 0) currentEnemyCount = 0;


        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = Grid.adjustWoldPosToNearestCell(worldPos, grid.gridCellSize);

        //add adjusted world position to grid
        int newCorpseNum = grid.addWorldPosToArray(adjustedPos, corpsenumber);

        //if (newCorpseNum == 99)
        //{
        //    removeCorpseAtWorldPos(adjustedPos);
        //}
        if (newCorpseNum == 999)
        {
            removeCorpseAtWorldPos(adjustedPos);
            return new CoprseInfoObject(newCorpseNum, adjustedPos, corpseMoundObj);
        }

        //check if pattern is found in grid
        List<Vector2Int> fitPatter = patternChecker.checkForPatternAndReturnPositions(pattern, grid.array);


        //if pattern is found - remove objects and move game along
        if (fitPatter.Count > 0)
        {
            int multiplier = 1;
            Vector3 doublerLoc;

            if (doubler == null || doubler.transform == null)
            {
                doublerLoc = new Vector3(999, 999, 999);
                Destroy(doubler);
                SpawnDoubler();
            }
            else
            {
                Vector3 doublerLocAdjusted = Grid.adjustWoldPosToNearestCell(doubler.transform.position, grid.gridCellSize);
                doublerLoc = grid.ConvetWorldPosToArrayPos(doublerLocAdjusted);
            }

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
                if (grid.array[corpseLoc.x,corpseLoc.y] < 199 || grid.array[corpseLoc.x, corpseLoc.y] > 299)
                {
                    removeCorpseAtGridLoc(corpseLoc);
                }

            }
            success = true;
            Debug.Log("SUCCESS");
            Vector2Int middle = fitPatter[(fitPatter.Count - 1) / 2];
            float size = pattern.GetLength(0) > pattern.GetLength(1) ? pattern.GetLength(0) : pattern.GetLength(1);
            GameObject effect = Instantiate(scoreEffect, grid.getWorldPositionGridWithOffset(middle.x, middle.y) + new Vector3(gridCellSize / 2, gridCellSize / 2), Quaternion.identity);
            effect.transform.localScale = effect.transform.localScale * size;
            //get new random pattern from store

            //Increment score and set UI
            incrementScore(multiplier);

            if (currentLevel != Level.ENDLESS)
            {
                if (patternList.Count() < 1)
                {
                    if (levelConfigs[currentLevel].bossType != null)
                    {
                        if (!bossmode)
                        {
                            //spawn snakeboss
                            setBoss();
                            bossmode = true;
                        }
                        else
                        {
                            if (corpsenumber > 100 && GameObject.FindObjectsOfType(levelConfigs[currentLevel].bossType).Count() == 1)
                            //if (corpsenumber > 100 && GameObject.FindObjectsOfType(boss.GetType()).Count() == 1)
                            {
                                //killed last snake. Level Complete
                                StartCoroutine(Win());
                                //return the world position where the enemy should place the coprse
                                return new CoprseInfoObject(newCorpseNum, adjustedPos, null);
                            }else if ( currentLevel == Level.LEVEL1_2)
                            {
                                StartCoroutine(Win());
                                //return the world position where the enemy should place the coprse
                                return new CoprseInfoObject(newCorpseNum, adjustedPos, null);
                            }
                            else
                            {
                                String bossName = GameObject.FindObjectOfType(levelConfigs[currentLevel].bossType).name;
                                boss = GameObject.Find(bossName);
                            }
                        }
                        //pattern = (new int[1, 1] { { boss.GetComponent<Enemy>().corpseNumber } });
                        pattern = levelConfigs[currentLevel].bossPattern;
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
            if (!bossmode)
            {
                pattern = CorpseStore.Instance.spiceItUp(pattern, levelConfigs[currentLevel].spiceChance);

            }
            patternGrid.setPattern(pattern);



        }
        else
        {
            //boss dead, but pattern not commplete
            if (bossmode && corpsenumber > 100 && GameObject.FindObjectsOfType(levelConfigs[currentLevel].bossType).Count() == 1)
            {
                setBoss();
            }
        }

        //return the world position where the enemy should place the coprse
        return new CoprseInfoObject(newCorpseNum, adjustedPos, null);
    }

    private void setBoss()
    {
        if (levelConfigs[currentLevel].bossType == typeof(SnakeBoss))
        {
            boss = Instantiate(snakeBoss.gameObject, Vector3.zero, Quaternion.identity);
        }
        else if (levelConfigs[currentLevel].bossType == typeof(TurtleBoss))
        {
            boss = Instantiate(turtleBoss.gameObject, Vector3.zero, Quaternion.identity);
        }
        else if (levelConfigs[currentLevel].bossType == typeof(TheBombBoss))
        {
            boss = Instantiate(bombBoss.gameObject, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("THIS IS NOT SUPPOSED TO HAPPEN, NO BOSS GIVEN");
        }
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
            highscore = score * 10;
            PlayerPrefs.SetInt("HighScore_" + currentLevel.ToString(), highscore);
            hightText.SetText("Top:  " + highscore);
        }
        if ((player.level * 1) - 1 < score && score != 0 && (currentLevel == Level.ENDLESS || patternList.Count > 0))
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

    public void setGridCellFromWorldPos(int number, Vector3 pos)
    {

        //adjust fractional world pos to nearest multiple of gridcellsize AND offset it to center of Grid
        Vector3 adjustedPos = Grid.adjustWoldPosToNearestCell(pos, grid.gridCellSize);

        //add adjusted world position to grid
        int newCorpseNum = grid.addWorldPosToArray(adjustedPos, number);
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
        List<int> filtered = enemiesFromPattern.Where(code => code % 10 != 0 && code < 90 && code > 9).ToList();
        foreach (int item in aliveCodes)
        {
            // Remove the first occurrence of the item in list1 (if it exists)
            filtered.Remove(item);
        }
        if (UnityEngine.Random.Range(1, 10) <= 6)
        {
            foreach (int item in corpseCodes)
            {
                // Remove the first occurrence of the item in list1 (if it exists)
                filtered.Remove(item);
            }
        }
        //if not empty spawn in pattern
        if (filtered.Count > 0)
        {
            enemyToSpawn = EnemyStore.instance.getEnemyByCorpseCode(filtered.ToArray()[0]);
        }
        else
        {
            enemyToSpawn = EnemyStore.instance.getRandomEnemyWithMaxPower(maxPower);
        }

        currentEnemyCount += enemyToSpawn.powerLevel;

        Debug.Log("SPAWNING " + enemyToSpawn.name);

        if (player == null)
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
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(worldPos, 0.2f, LayerMask.GetMask("Enemy", "ObstacleBlock", "Player"));

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

    public void setBackgroundMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
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

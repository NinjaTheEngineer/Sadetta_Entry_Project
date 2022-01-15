using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private float airObstaclesMinY;
    [SerializeField] private float airObstaclesMaxY;

    [SerializeField] private float spawnTimer = 4f;
    [SerializeField] private float minSpawnTimer = 1f;
    [SerializeField] private float spawnTimeDecrementation = 0.1f;
    [SerializeField] private BackgroundController backgroundController;

    public delegate void GameEnded();
    public event GameEnded OnGameEnded;

    private float spawnTimerCounter;
    private ObjectPooler objectPooler;
    private UIManager uiManager;
    private List<string> obstacleTags;

    private Vector3 groundObstaclesPosition = new Vector3(10, -3, 0);
    private Vector3 airObstaclesPosition;
    private bool isPlayerAlive = true;

    public int score = 0;

    public static GameManager Instance;

    public void SaveHighScore(string name)
    {
        DataStorage.AddHighscoreEntry(score, name);
    }
    #endregion

    #region Monobehaviour
    private void Awake() //Simple Singleton
    {
        Instance = this;
    }

    private void Start() //Initialize variables
    {
        uiManager = UIManager.Instance;
        uiManager.SetScore(score);
        objectPooler = ObjectPooler.Instance;
        obstacleTags = new List<string>();
        foreach (Pool pool in objectPooler.Pools)
        {
            obstacleTags.Add(pool.tag);
        }
        spawnTimerCounter = spawnTimer;
    }

    private void Update() //If the player is alive spawns obstacles and handles ESC press
    {
        if (isPlayerAlive)
            SpawnObstacles();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }
    #endregion

    #region Methods
    private void SpawnObstacles() //Spawn Obstacles between a certain amount of time
    {
        if (spawnTimerCounter > 0)
        {
            spawnTimerCounter -= Time.deltaTime;
        }
        else
        {
            if (spawnTimer > minSpawnTimer) //With each spawn the time decreases
            {
                spawnTimer -= spawnTimeDecrementation;
            }
            spawnTimerCounter = spawnTimer;
            SpawnRandomObstacle();
        }
    }
    private void SpawnRandomObstacle() //Selects a random obstacle to spawn
    {
        int randomIndex = Random.Range(0, obstacleTags.Count);
        if(randomIndex < 2)
        {
            float randomY = Random.Range(airObstaclesMinY, airObstaclesMaxY);
            airObstaclesPosition = new Vector3(10, randomY, 0);
            objectPooler.SpawnFromPool(obstacleTags[randomIndex], airObstaclesPosition, Quaternion.identity);
        }
        else
        {
            objectPooler.SpawnFromPool(obstacleTags[randomIndex], groundObstaclesPosition, Quaternion.identity);
        }
    }
    public void PlayerCrashed() //Called when Player Crashes
    {
        isPlayerAlive = false;
        OnGameEnded();
        uiManager.PlayerGotHighscore(DataStorage.CheckIfOnTop5(score));
    }
    public void IncrementScore() //Increments the score for each obstacle passed
    {
        score++;
        uiManager.SetScore(score);
    }

    private void ReturnToMainMenu()
    {
        LevelLoader.Instance.LoadMainMenu();
    }

    public void PlayAgain()
    {
        LevelLoader.Instance.RestartLevel();
    }
    #endregion
}

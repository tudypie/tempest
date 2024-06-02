using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnityEvent onLevelEnd;

    [Header("References")]
    [SerializeField] private GameObject[] enemies;

    [Header("Spawning Settings")]
    [SerializeField] private float beginSpawningDelay = 3;
    [SerializeField] private float minSpawnTime, maxSpawnTime;
    [SerializeField] private float spawnTimeDecrease = 0.02f;
    [SerializeField] private float currentSpawnTime;
    public float spawningDuration;
    private bool startedSpawning = false;

    [Header("Enemy Settings")]
    [SerializeField] private bool keepRotation;
    [SerializeField] private bool invertRotation;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 rotationOffset;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private GameManager gameManager;
    private Wireframe wireframe;

    public static EnemySpawner Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        currentSpawnTime = maxSpawnTime;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        wireframe = Wireframe.Instance;
    }

    private void Update()
    {
        if (!gameManager.ongoingGame) return;

        if(beginSpawningDelay > 0)
        {
            beginSpawningDelay -= Time.deltaTime;
            return;
        }

        if (spawningDuration > 0)
            spawningDuration -= Time.deltaTime;

        if (!startedSpawning)
        {
            StartCoroutine(SpawningSequence());
            startedSpawning = true;
        }
    }

    private void SpawnEnemy()
    {
        int rndLine = Random.Range(0, wireframe.lines.Length);
        int rndEnemy = Random.Range(0, enemies.Length);
        spawnedEnemies.Add(gameManager.SpawnObjectOnMap(enemies[rndEnemy], rndLine, spawnOffset, keepRotation, invertRotation));
    }

    public void DestroyAllEnemies()
    {
        foreach(GameObject enemy in spawnedEnemies) {
            Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

    private IEnumerator SpawningSequence()
    {
        while(gameManager.ongoingLevel && (spawningDuration > 0 || gameManager.LevelHasBossfight))
        {
            yield return new WaitForSeconds(currentSpawnTime);
            SpawnEnemy();
            currentSpawnTime = Mathf.Clamp(currentSpawnTime - spawnTimeDecrease, minSpawnTime, maxSpawnTime);
        }
    }

    public void OnLevelEnd()
    {
        DestroyAllEnemies();
        onLevelEnd?.Invoke();
    }
}

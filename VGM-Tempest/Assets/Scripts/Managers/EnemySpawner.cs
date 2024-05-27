using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemy;

    [Header("Spawning Settings")]
    [SerializeField] private float beginSpawningDelay = 3;
    [SerializeField] private float spawningDuration = 5;
    [SerializeField] private float spawnDelay = 0.3f;
    private float currentSpawnDelay;

    [Header("Enemy Settings")]
    [SerializeField] private bool keepRotation;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private Vector3 rotationOffset;

    public bool CanSpawn { get; set; } = true;

    private GameManager gameManager;

    public static EnemySpawner instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (!gameManager.gameStarted || !CanSpawn) return;

        if(beginSpawningDelay > 0)
        {
            beginSpawningDelay -= Time.deltaTime;
            return;
        }

        if (spawningDuration > 0)
        {
            spawningDuration -= Time.deltaTime;
            if (currentSpawnDelay > 0)
            {
                currentSpawnDelay -= Time.deltaTime;
            }
            else
            {
                SpawnEnemy();
                currentSpawnDelay = spawnDelay;
            }
        }
    }

    private void SpawnEnemy()
    {
        int rndLine = Random.Range(0, gameManager.wireframeLine.Length);
        int rndEnemy = Random.Range(0, enemy.Length);
        gameManager.SpawnObjectOnMap(enemy[rndEnemy], rndLine, spawnOffset, keepRotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    private float currentSpawnDelay;
    [SerializeField] private bool keepRotation;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private Vector3 spawnOffset;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if(currentSpawnDelay > 0)
        {
            currentSpawnDelay -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            currentSpawnDelay = spawnDelay;
        }
    }

    private void SpawnEnemy()
    {
        int rndSpawn = Random.Range(0, gameManager.movePoint.Length);
        gameManager.SpawnObjectOnMap(enemy[0], rndSpawn, spawnOffset, keepRotation);
    }
}

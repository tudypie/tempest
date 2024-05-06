using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnpoint;
    [SerializeField] private GameObject[] enemy;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float spawnTime;
    [SerializeField] private bool followRotation;
    float spawnDelay;

    private void Update()
    {
        if(spawnDelay > 0)
        {
            spawnDelay -= Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnDelay = spawnTime;
        }
    }

    private void SpawnEnemy()
    {
        int rndSpawn = Random.Range(0, spawnpoint.Length);
        Instantiate(enemy[0], spawnpoint[rndSpawn].position + spawnOffset, 
            followRotation ? spawnpoint[rndSpawn].rotation : Quaternion.identity);
    }
}

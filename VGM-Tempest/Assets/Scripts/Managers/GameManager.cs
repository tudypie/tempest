using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playersInGame = 0;
    public Material wireframeMaterial;
    public Material[] playerMaterial;
    public Transform[] movePoint;

    [HideInInspector] public AudioManager audioManager;
    [HideInInspector] public ScoreManager scoreManager;
    [HideInInspector] public TextManager textManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioManager = GetComponent<AudioManager>();
        scoreManager = GetComponent<ScoreManager>();
        textManager = GetComponent<TextManager>();
    }

    public void OnPlayerJoin(PlayerManager player)
    {
        player.playerNumber = playersInGame;
        player.playerMaterial = playerMaterial[playersInGame];
        playersInGame++;
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int movePointIndex, Vector3 spawnOffset, bool keepRotation = false)
    {
        return Instantiate(objectToSpawn, movePoint[movePointIndex].position + spawnOffset, 
            keepRotation ? Quaternion.identity : movePoint[movePointIndex].rotation);
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int movePointIndex, Vector3 spawnOffset, Vector3 rotationOffset, Vector3 objectScale, bool keepRotation = false)
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, movePoint[movePointIndex].position + spawnOffset,
            keepRotation ? Quaternion.identity : movePoint[movePointIndex].rotation);
        spawnedObject.transform.Rotate(rotationOffset);
        spawnedObject.transform.localScale = objectScale;
        return spawnedObject;
    }
}

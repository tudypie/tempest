using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playersInGame = 0;
    public Material wireframeMaterial;
    public Material[] playerMaterial;
    public Material[] playerLineMaterial;
    public Transform[] wireframeLine;

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

    public void SetLineColor(int index, int player)
    {
        wireframeLine[index].parent.GetComponent<Renderer>().material = playerLineMaterial[player];
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, bool keepRotation = false)
    {
        return Instantiate(objectToSpawn, wireframeLine[wireframeLineIndex].position + spawnOffset, 
            keepRotation ? Quaternion.identity : wireframeLine[wireframeLineIndex].rotation);
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, Vector3 rotationOffset, Vector3 objectScale, bool keepRotation = false)
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, wireframeLine[wireframeLineIndex].position + spawnOffset,
            keepRotation ? Quaternion.identity : wireframeLine[wireframeLineIndex].rotation);
        spawnedObject.transform.Rotate(rotationOffset);
        spawnedObject.transform.localScale = objectScale;
        return spawnedObject;
    }
}

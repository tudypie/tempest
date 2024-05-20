using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private string textMessage;
    [SerializeField] private float delayBetweenCharacters = 0.4f;
    [SerializeField] private Vector3 spawnPositionOffset;
    [SerializeField] private Vector3 spawnRotationOffset;
    [SerializeField] private char[] characterKey;
    [SerializeField] private GameObject[] characterPrefab;

    private Dictionary<char, GameObject> characterDictionary = new Dictionary<char, GameObject>();

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        for(int i = 0; i < characterKey.Length; i++)
        {
            characterDictionary.Add(characterKey[i], characterPrefab[i]);
        }
        SpawnTextMessage();
    }

    public void SpawnTextMessage()
    {
        StartCoroutine(SpawningSequence());
    }

    private IEnumerator SpawningSequence()
    {
        foreach (char c in textMessage)
        {
            yield return new WaitForSeconds(delayBetweenCharacters);
            GameObject letter = gameManager.SpawnObjectOnMap(characterDictionary[c], 5, spawnPositionOffset);
            letter.transform.Rotate(spawnRotationOffset);
        }
    }
}

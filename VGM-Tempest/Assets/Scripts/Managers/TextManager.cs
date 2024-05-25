using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [Serializable]
    public struct TextMessage
    {
        [SerializeField]
        [TextArea(5, 10)]
        public string message;
        public int movePoint;
        public float duration;
        public Vector3 spawnOffset;
    }

    [SerializeField] private TextMessage[] textMessages;
    [SerializeField] private int moveSpawnPoint;
    [SerializeField] private float delayBetweenCharacters = 0.4f;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private char[] characterKey;
    [SerializeField] private Mesh[] characterMesh;
    private GameObject spawnedCharacter;
    private Dictionary<char, Mesh> characterDictionary = new Dictionary<char, Mesh>();

    private GameManager gameManager;

    public static TextManager Instance;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < characterKey.Length; i++)
            characterDictionary.Add(characterKey[i], characterMesh[i]);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.gameStarted)
        {
            StartCoroutine(SpawningTexts());
        }
    }

    public IEnumerator SpawningTexts()
    {
        foreach (TextMessage message in textMessages)
        {
            StartCoroutine(SpawningSequence(message));
            yield return new WaitForSeconds(message.duration);
        }
    }

    private IEnumerator SpawningSequence(TextMessage message)
    {
        foreach (char c in message.message)
        {
            yield return new WaitForSeconds(delayBetweenCharacters);
            if (c == ' ' || !characterDictionary.ContainsKey(char.ToUpper(c))) continue;
            spawnedCharacter = gameManager.SpawnObjectOnMap(characterPrefab, message.movePoint, message.spawnOffset);
            spawnedCharacter.transform.GetChild(0).Rotate(rotationOffset);
            spawnedCharacter.GetComponentInChildren<MeshFilter>().mesh = characterDictionary[char.ToUpper(c)];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField][TextArea(5, 10)]
    private string textMessage;
    [SerializeField] private int moveSpawnPoint;
    [SerializeField] private float delayBetweenCharacters = 0.4f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;
    //[SerializeField] private Vector3 characterScale;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private char[] characterKey;
    [SerializeField] private Mesh[] characterMesh;
    private GameObject spawnedCharacter;

    private Dictionary<char, Mesh> characterDictionary = new Dictionary<char, Mesh>();

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        for(int i = 0; i < characterKey.Length; i++)
        {
            characterDictionary.Add(characterKey[i], characterMesh[i]);
        }
        SpawnTextMessage();
    }

    private void Update()
    {

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
            if (c == ' ' || !characterDictionary.ContainsKey(char.ToUpper(c)))
            {
                Debug.Log(c);
                continue;
            }
            spawnedCharacter = gameManager.SpawnObjectOnMap(characterPrefab, moveSpawnPoint, positionOffset);
            spawnedCharacter.transform.GetChild(0).Rotate(rotationOffset);
            //spawnedCharacter.transform.GetChild(0).localScale = characterScale;
            spawnedCharacter.GetComponentInChildren<MeshFilter>().mesh = characterDictionary[char.ToUpper(c)];
        }
    }
}

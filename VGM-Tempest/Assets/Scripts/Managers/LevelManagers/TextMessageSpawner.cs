using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessageSpawner : MonoBehaviour
{
    [Header("Text Messages")]
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private TextMessagesSO textMessages;

    [Header("Spawn Settings")]
    [SerializeField] private float beginSpawningDelay = 6f;
    [SerializeField] private float delayBetweenCharacters = 0.4f;
    [SerializeField] private Vector3 rotationOffset;

    [Header("Characters Dictionary")]
    [SerializeField] private char[] characterKey;
    [SerializeField] private Mesh[] characterMesh;

    [Header("End Level Messages")]
    [SerializeField] private Text endLevelText;
    [SerializeField] private float delayBetweenMessages;
    [SerializeField][TextArea(5, 10)] private string[] endLevelMessages;
    [SerializeField] private float typeWriterEffectDelay = 0.04f;

    private GameObject spawnedCharacter;
    private Dictionary<char, Mesh> characterDictionary = new Dictionary<char, Mesh>();

    private GameManager gameManager;

    public bool FinishedMessages { get; private set; } = false;

    public static TextMessageSpawner Instance;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < characterKey.Length; i++)
            characterDictionary.Add(characterKey[i], characterMesh[i]);

        endLevelText.text = string.Empty;
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public IEnumerator ShowEndLevelText()
    {
        FinishedMessages = false;
        foreach(string message in endLevelMessages)
        {
            StartCoroutine(TypeWriterEffect(message));
            yield return new WaitForSeconds(delayBetweenMessages);
        }
        FinishedMessages = true;
    }

    public IEnumerator SpawningSequence()
    {
        if(textMessages != null)
        {
            yield return new WaitForSeconds(beginSpawningDelay);
            foreach (TextMessagesSO.Message message in textMessages.messages)
            {
                StartCoroutine(SpawnTextMessage(message));
                yield return new WaitForSeconds(delayBetweenCharacters * message.text.Length);
                yield return new WaitForSeconds(textMessages.delay);
            }
        }
    }

    private IEnumerator SpawnTextMessage(TextMessagesSO.Message message)
    {
        int rndSpawnpoint = UnityEngine.Random.Range(0, gameManager.wireframe.lines.Length - 1);
        while(rndSpawnpoint == 0 || rndSpawnpoint == 1 || rndSpawnpoint == 9 || rndSpawnpoint == 10)
            rndSpawnpoint = UnityEngine.Random.Range(0, gameManager.wireframe.lines.Length - 1);

        if (rndSpawnpoint > 0 && rndSpawnpoint < 10) 
            message.text = Reverse(message.text);

        foreach (char c in message.text)
        {
            if (characterDictionary.ContainsKey(char.ToUpper(c)))
            {
                spawnedCharacter = gameManager.SpawnObjectOnMap(characterPrefab, rndSpawnpoint, textMessages.spawnOffset);
                Transform child = spawnedCharacter.transform.GetChild(0);
                child.Rotate(rotationOffset);
                if (rndSpawnpoint > 0 && rndSpawnpoint < 10)
                    child.localScale = new Vector3(-child.localScale.x, -child.localScale.y, child.localScale.z);
                spawnedCharacter.GetComponentInChildren<MeshFilter>().mesh = characterDictionary[char.ToUpper(c)];
            }
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }

    private IEnumerator TypeWriterEffect(string fullText)
    {
        string currentText;
        for (int i = 0; i < fullText.Length + 1; i++)
        {
            currentText = fullText.Substring(0, i);
            endLevelText.text = currentText;
            yield return new WaitForSeconds(typeWriterEffectDelay);
        }
    }

    public string Reverse(string text)
    {
        char[] cArray = text.ToCharArray();
        string reverse = String.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }
}

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
                yield return new WaitForSeconds(message.delay);
            }
        }
    }

    private IEnumerator SpawnTextMessage(TextMessagesSO.Message message)
    {
        foreach (char c in message.text)
        {
            if (characterDictionary.ContainsKey(char.ToUpper(c)))
            {
                spawnedCharacter = gameManager.SpawnObjectOnMap(characterPrefab, message.lineSpawnPoint, message.spawnOffset);
                spawnedCharacter.transform.GetChild(0).Rotate(rotationOffset);
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
}

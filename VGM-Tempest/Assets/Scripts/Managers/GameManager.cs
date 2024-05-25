using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct GameLevel
    {
        public string name;
        public float duration;
        public bool bossfight;
    }

    [Header("References")]
    public Transform[] wireframeLine;
    public Material wireframeMaterial;
    [SerializeField] private Material[] playerMaterial;
    [SerializeField] private Material[] playerLineMaterial;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private Text endLevelText;

    [Header("Game Settings")]
    [SerializeField] private GameLevel[] levels;
    [SerializeField] [TextArea(5, 10)] 
    private string[] endLevelMessages;
    [SerializeField] private float typeWriterEffectDelay = 0.04f;
    [SerializeField] private float currentLevelDuration;
    [SerializeField] private int currentLevelCount;
    [SerializeField] int playersInGame = 0;
    [SerializeField] private bool finishedTypeWriterEffect = false;
    public bool gameStarted = false;

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

        currentLevelDuration = levels[currentLevelCount].duration;
    }

    private void Start()
    {
        textManager = TextManager.Instance;
    }

    private void Update()
    {
        if (!gameStarted) return;

        if(currentLevelDuration > 0)
        {
            currentLevelDuration -= Time.deltaTime;
        }
        else if (!levels[currentLevelCount].bossfight)
        {
            NextLevel();
        }
    }

    public void NextLevel()
    {
        if (currentLevelCount < levels.Length - 1)
        {
            currentLevelCount++;
        }
        else
        {
            return;
        }
        currentLevelDuration = levels[currentLevelCount].duration;
        StartCoroutine(NextLevel(levels[currentLevelCount]));
    }

    public void OnPlayerJoin(PlayerManager player)
    {
        if (currentLevelCount == 0 && playersInGame == 0)
        {
            startCanvas.SetActive(false);
            playerCanvas.SetActive(true);
            Camera.main.GetComponent<Animator>().Play("BeginLevel");
            textManager.StartCoroutine(textManager.SpawningTexts());
            gameStarted = true;          
        }

        player.playerNumber = playersInGame;
        player.playerMaterial = playerMaterial[playersInGame];
        player.movement.currentPointIndex = playersInGame == 0 ? 14 : 6;
        player.movement.SetPlayerOnWireframeLine();
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

    private IEnumerator NextLevel(GameLevel level)
    {
        Camera.main.GetComponent<Animator>().Play("NextLevel");
        yield return new WaitForSeconds(2f);

        finishedTypeWriterEffect = false;
        StartCoroutine(TypeWriterEffect(endLevelMessages[Array.IndexOf(levels, level)]));
        while(!finishedTypeWriterEffect)
            yield return null;

        yield return new WaitForSeconds(2f);
        Camera.main.GetComponent<Animator>().Play("NextLevel2");
        yield return new WaitForSeconds(3f);
        endLevelText.text = string.Empty;
        SceneManager.LoadScene(level.name);
        yield return new WaitForSeconds(2f);
        Camera.main.GetComponent<Animator>().Play("BeginLevel");
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
        finishedTypeWriterEffect = true;
    }
}

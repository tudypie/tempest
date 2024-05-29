using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct GameLevel
    {
        public string name;
        public float duration;
        public bool hasBossfight;
    }

    [Header("References")]
    [SerializeField] private Material[] playerMaterial;
    [SerializeField] private Material[] playerLineMaterial;

    [Header("Levels")]
    [SerializeField] private GameLevel[] levels;

    [Header("Debug")]
    [SerializeField] private int numOfPlayers = 0;
    [SerializeField] private int currentLevel;
    [SerializeField] private float levelDuration;

    [SerializeField] private PlayerManager[] playersInGame = new PlayerManager[2];

    public bool ongoingGame = false;
    public bool ongoingLevel = false;

    private TextMessageSpawner textSpawner;
    private EnemySpawner enemySpawner;
    private Camera mainCamera;

    public Wireframe wireframe { get; private set; }
    public AudioManager audioManager { get; private set; }
    public ScoreManager scoreManager { get; private set; }
    public UIManager uiManager { get; private set; }

    public bool LevelHasBossfight { get { return levels[currentLevel].hasBossfight; } }

    public bool TwoPlayersInGame { get { return numOfPlayers == 2; } }

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioManager = GetComponent<AudioManager>();
        scoreManager = GetComponent<ScoreManager>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        wireframe = Wireframe.Instance;
        textSpawner = TextMessageSpawner.Instance;
        enemySpawner = EnemySpawner.Instance;
        mainCamera = Camera.main;

        if (ongoingGame)
            StartGame();
    }

    private void Update()
    {
        if (!ongoingGame || !ongoingLevel) return;

        if (levelDuration > 0)
        {
            levelDuration -= Time.deltaTime;
        }
        else if (!levels[currentLevel].hasBossfight)
        {
            NextLevel();
        }
    }

    private void StartGame()
    {
        uiManager.ActivateStartCanvas(false);
        uiManager.ActivatePlayerCanvas(true);
        ongoingGame = true;
        StartLevel();
    }

    private void StartLevel()
    {
        wireframe = Wireframe.Instance;
        textSpawner = TextMessageSpawner.Instance;
        enemySpawner = EnemySpawner.Instance;
        mainCamera = Camera.main;

        mainCamera.GetComponent<Animator>().Play("BeginLevel");
        levelDuration = levels[currentLevel].duration;
        enemySpawner.spawningDuration = levelDuration - 5;
        ongoingLevel = true;
    }

    private void EndGame()
    {
        uiManager.ActivateEndCanvas(true);
        scoreManager.CalculateTotalScore();
        ongoingGame = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);

        StartCoroutine(DelayedRemovePlayer());

        numOfPlayers = 0;
        currentLevel = 0;
        scoreManager.ResetScore();

        uiManager.ActivateEndCanvas(false);
        uiManager.ActivatePlayerCanvas(false);
        uiManager.ActivateStartCanvas(true);

        wireframe = Wireframe.Instance;
        textSpawner = TextMessageSpawner.Instance;
        enemySpawner = EnemySpawner.Instance;
        mainCamera = Camera.main;
    }
    private IEnumerator DelayedRemovePlayer()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Destroy(playersInGame[0].gameObject);
        if(playersInGame[1] != null) Destroy(playersInGame[1].gameObject);
        yield return null;
    }


    public void NextLevel()
    {
        ongoingLevel = false;
        currentLevel++;
        StartCoroutine(LevelTransition(currentLevel));
    }

    public void OnPlayerJoin(PlayerManager player)
    {
        if (currentLevel == 0 && numOfPlayers == 0)
            StartGame();

        playersInGame[numOfPlayers] = player;
        player.playerNumber = numOfPlayers;
        player.playerMaterial = playerMaterial[numOfPlayers];
        numOfPlayers++;
    }

    public void SetPlayerLineColor(int index, int player)
    {
        if (wireframe != null) 
            wireframe.SetLineMaterial(index, playerLineMaterial[player]);
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, bool keepRotation = false, bool invertRotation = false)
    {
        if (wireframe == null) return null;

        if(invertRotation && (wireframeLineIndex == 5 || wireframeLineIndex == 14)) 
            keepRotation = true;

        Transform spawnedObject = Instantiate(objectToSpawn, wireframe.lines[wireframeLineIndex].position + spawnOffset, 
            keepRotation ? Quaternion.identity : wireframe.lines[wireframeLineIndex].rotation).transform;

        if(invertRotation && wireframeLineIndex > 5 && wireframeLineIndex < 14)
            spawnedObject.localScale =  new Vector3(spawnedObject.localScale.x, -spawnedObject.localScale.y, spawnedObject.localScale.z);

        return spawnedObject.gameObject;
     }

    private IEnumerator LevelTransition(int level)
    {
        enemySpawner.DestroyAllEnemies();
        mainCamera.GetComponent<Animator>().Play("NextLevel");
        yield return new WaitForSeconds(2f);

        textSpawner.ShowEndLevelText();
        while(!textSpawner.FinishedTypeWriterEffect)
            yield return null;

        yield return new WaitForSeconds(2f);
        mainCamera.GetComponent<Animator>().Play("NextLevel2");
        yield return new WaitForSeconds(3f);

        if(level < levels.Length)
        {
            SceneManager.LoadScene(levels[level].name);
            yield return new WaitForSeconds(1f);
            playersInGame[0].movement.currentLine = Wireframe.Instance.lines.Length / 2;
            playersInGame[0].movement.SetPlayerOnWireframeLine();
            if (playersInGame[1] != null)
            {
                playersInGame[1].movement.currentLine = Wireframe.Instance.lines.Length / 2;
                playersInGame[1].movement.SetPlayerOnWireframeLine();
            }
            StartLevel();
        }
        else
        {
            SceneManager.LoadScene("EndScreen");
            yield return new WaitForSeconds(1f);
            EndGame();
        }

    }
}

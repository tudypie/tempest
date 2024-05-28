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
    [SerializeField] private int playersInGame = 0;
    [SerializeField] private int currentLevel;
    [SerializeField] private float levelDuration;
    [SerializeField] private bool levelStarted;

    public bool gameStarted = false;

    private TextMessageSpawner textSpawner;
    private EnemySpawner enemySpawner;
    private Camera mainCamera;

    public Wireframe wireframe { get; private set; }
    public AudioManager audioManager { get; private set; }
    public ScoreManager scoreManager { get; private set; }
    public UIManager uiManager { get; private set; }

    public bool LevelHasBossfight { get { return levels[currentLevel].hasBossfight; } }

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
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        wireframe = Wireframe.Instance;
        textSpawner = TextMessageSpawner.Instance;
        enemySpawner = EnemySpawner.Instance;
        mainCamera = Camera.main;

        if (gameStarted)
            StartGame();
    }

    private void Update()
    {
        if (!gameStarted) return;

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
        gameStarted = true;
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
        levelStarted = true;
    }

    private void EndGame()
    {

    }

    public void NextLevel()
    {
        if (currentLevel < levels.Length - 1)
        {
            currentLevel++;
            levelStarted = false;
        }
        else
        {
            EndGame();
            return;
        }
        StartCoroutine(LevelTransition(levels[currentLevel]));
    }

    public void OnPlayerJoin(PlayerManager player)
    {
        if (currentLevel == 0 && playersInGame == 0)
            StartGame();

        player.playerNumber = playersInGame;
        player.playerMaterial = playerMaterial[playersInGame];
        playersInGame++;
    }

    public void SetPlayerLineColor(int index, int player)
    {
        if (wireframe != null) 
            wireframe.SetLineMaterial(index, playerLineMaterial[player]);
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, bool keepRotation = false)
    {
        if (wireframe == null) return null;

        return Instantiate(objectToSpawn, wireframe.lines[wireframeLineIndex].position + spawnOffset, 
            keepRotation ? Quaternion.identity : wireframe.lines[wireframeLineIndex].rotation);
    }

    public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, Vector3 rotationOffset, Vector3 objectScale, bool keepRotation = false)
    {
        if (wireframe == null) return null;

        GameObject spawnedObject = Instantiate(objectToSpawn, wireframe.lines[wireframeLineIndex].position + spawnOffset,
            keepRotation ? Quaternion.identity : wireframe.lines[wireframeLineIndex].rotation);
        spawnedObject.transform.Rotate(rotationOffset);
        spawnedObject.transform.localScale = objectScale;
        return spawnedObject;
    }

    /*public GameObject SpawnObjectOnMap(GameObject objectToSpawn, int wireframeLineIndex, Vector3 spawnOffset, int rotateOnAxis)
    {
        Transform line = wireframeLine[wireframeLineIndex];
        GameObject spawnedObject = Instantiate(objectToSpawn, wireframeLine[wireframeLineIndex].position + spawnOffset, Quaternion.identity);

        spawnedObject.transform.Rotate(new Vector3(
            rotateOnAxis == 0 ? line.position.x : 0,
            rotateOnAxis == 1 ? line.position.y : 0,
            rotateOnAxis == 2 ? line.position.z : 0
            ));

        return spawnedObject;
    }*/

    private IEnumerator LevelTransition(GameLevel level)
    {
        mainCamera.GetComponent<Animator>().Play("NextLevel");
        yield return new WaitForSeconds(2f);

        textSpawner.ShowEndLevelText();
        while(!textSpawner.FinishedTypeWriterEffect)
            yield return null;

        yield return new WaitForSeconds(2f);
        mainCamera.GetComponent<Animator>().Play("NextLevel2");
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(level.name);

        yield return new WaitForSeconds(1f);
        StartLevel();
    }
}

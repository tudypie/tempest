using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerShooting shooting;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public AudioSource audioSource;

    public int playerNumber;
    public Material playerMaterial;

    private GameManager gameManager;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnPlayerJoin(this);
        meshRenderer.material = playerMaterial;
    }
}

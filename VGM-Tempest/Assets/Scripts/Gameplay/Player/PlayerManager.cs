using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerShooting shooting;
    [HideInInspector] public MeshRenderer meshRenderer;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Animator anim;

    public int playerNumber;
    public Material playerMaterial;
    public int scoreLoseOnHit = -3;
    public Text scoreDecreaseText;

    private GameManager gameManager;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        meshRenderer.material = playerMaterial;
    }

    public void TakeDamage()
    {
        gameManager.scoreManager.AddScore(playerNumber, scoreLoseOnHit);
        gameManager.audioManager.PlaySound(audioSource, gameManager.audioManager.playerTakeDamage);
        anim.Play("PlayerTakeDamage");
    }
}

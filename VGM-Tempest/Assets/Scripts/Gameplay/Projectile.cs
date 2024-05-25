using UnityEngine;

public class Projectile : MovingObject
{
    [SerializeField] private float damage;
    [HideInInspector] public int playerNumber;

    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private GameManager gameManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            gameManager.scoreManager.AddScore(playerNumber);
            gameManager.audioManager.PlaySoundWithRandomPitch(audioSource, gameManager.audioManager.explosion, 60, 180);
            Destroy(other.gameObject);
            meshRenderer.enabled = false;
            speed = 0;
            Destroy(gameObject, 3f);
        }

        if (other.TryGetComponent(out Boss boss))
        {
            boss.TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("BackBarrier"))
        {
            Destroy(gameObject);
        }
    }
}

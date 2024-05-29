using UnityEngine;

public class Projectile : MovingObject
{
    [SerializeField] private float damage;
    [HideInInspector] public int playerNumber;

    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private GameManager gameManager;

    private bool disabled = false;

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
        if (disabled) return;

        if(other.CompareTag("Enemy"))
        {
            other.TryGetComponent(out Enemy enemy);
            gameManager.scoreManager.AddScore(playerNumber);
            gameManager.audioManager.PlaySoundWithRandomPitch(audioSource, enemy.deathSound, 60, 180);
            Destroy(other.gameObject);

            speed = 0;
            meshRenderer.enabled = false;
            disabled = true;
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

using UnityEngine;

public class Projectile : MovingObject
{
    [SerializeField] private float damage;
    [HideInInspector] public int playerNumber;

    private GameManager gameManager;

    private void Awake()
    {
        Destroy(gameObject, 3f);
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
            gameManager.audioManager.PlaySoundWithRandomPitch(gameManager.audioManager.audioSource, gameManager.audioManager.explosion, 60, 180);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.TryGetComponent(out Boss boss))
        {
            boss.TakeDamage(damage);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " was hit");
        }

        if (other.gameObject.CompareTag("BackBarrier"))
        {
            Destroy(gameObject);
        }
    }
}

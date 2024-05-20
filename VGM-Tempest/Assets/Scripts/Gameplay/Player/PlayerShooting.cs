using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float shootDelay;
    private float currentShootDelay = 0f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Vector3 spawnOffset;

    private PlayerManager playerManager;
    private GameManager gameManager;
    private AudioSource audioSource;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentShootDelay > 0) return;
            Shoot();
            currentShootDelay = shootDelay;
        }
    }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (currentShootDelay > 0)
        {
            currentShootDelay -= Time.deltaTime;
            return;
        }
    }

    private void Shoot()
    {
        GameObject newProjectile = gameManager.SpawnObjectOnMap(projectile, playerManager.movement.CurrentPointIndex, spawnOffset);
        newProjectile.GetComponentInChildren<Renderer>().material = playerManager.playerMaterial;
        newProjectile.GetComponent<Projectile>().playerNumber = playerManager.playerNumber;
        AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.shoot);
    }
}

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

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentShootDelay > 0 || gameManager == null) return;
            Shoot();
            currentShootDelay = shootDelay;
        }
    }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
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
        GameObject newProjectile = gameManager.SpawnObjectOnMap(projectile, playerManager.movement.currentLine, spawnOffset);
        newProjectile.GetComponentInChildren<Renderer>().material = playerManager.playerMaterial;
        newProjectile.GetComponent<Projectile>().playerNumber = playerManager.playerNumber;
        gameManager.audioManager.PlaySound(playerManager.audioSource, gameManager.audioManager.shoot);
    }
}

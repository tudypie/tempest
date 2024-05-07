using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float shootDelay;

    float delay = 0f;

    private Movement movement;
    private AudioSource audioSource;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (delay > 0) return;

            Shoot();
            delay = shootDelay;
        }
    }

    private void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, PlayerManager.Instance.movePoint[movement.CurrentPointIndex].position + spawnOffset, 
            PlayerManager.Instance.movePoint[movement.CurrentPointIndex].rotation);

        newProjectile.GetComponentInChildren<Renderer>().material = movement.playerMaterial;
        newProjectile.GetComponent<Projectile>().playerNumber = movement.playerNumber;

        AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.shoot);
    }
}

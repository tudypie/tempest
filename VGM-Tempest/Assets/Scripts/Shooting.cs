using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private int playerNumber;
    [SerializeField] private GameObject projectile;
    [SerializeField] private KeyCode shootKey;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float shootDelay;

    float delay = 0f;

    private Movement movement;
    private Material playerMaterial;
    private AudioSource audioSource;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        playerMaterial = GetComponentInChildren<Renderer>().material;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        if (Input.GetKey(shootKey))
        {
            Shoot();
            delay = shootDelay;
        }
    }

    private void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, movement.movePoint[movement.CurrentPointIndex].position + spawnOffset, 
            movement.movePoint[movement.CurrentPointIndex].rotation);

        newProjectile.GetComponentInChildren<Renderer>().material = playerMaterial;
        newProjectile.GetComponent<Projectile>().playerNumber = playerNumber;

        AudioManager.Instance.PlaySound(audioSource, AudioManager.Instance.shoot);
    }
}

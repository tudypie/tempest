using UnityEngine;

public class Enemy : MovingObject
{
    public AudioClip deathSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerManager playerManager))
        {
            playerManager.TakeDamage();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("FrontBarrier"))
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Enemy : MovingObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerManager playerManager))
        {
            GameManager.Instance.scoreManager.AddScore(playerManager.playerNumber, -10);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("FrontBarrier"))
        {
            Destroy(gameObject);
        }
    }
}

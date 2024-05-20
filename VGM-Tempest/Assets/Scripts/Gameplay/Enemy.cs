using UnityEngine;

public class Enemy : MovingObject
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " was hit");
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("FrontBarrier"))
        {
            //AudioManager.Instance.PlaySound(AudioManager.Instance.audioSource, AudioManager.Instance.explosion);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public int playerNumber;

    private void Awake()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed* Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            ScoreManager.Instance.AddScore(playerNumber);
            AudioManager.Instance.PlaySoundWithRandomPitch(AudioManager.Instance.audioSource, AudioManager.Instance.explosion, 60, 180);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Boss"))
        {
            other.GetComponent<bOSS>().TakeDamage(damage);
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

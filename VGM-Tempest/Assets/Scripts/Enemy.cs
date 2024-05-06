using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name + " was hit");
        }

        if (other.gameObject.CompareTag("FrontBarrier"))
        {
            //AudioManager.Instance.PlaySound(AudioManager.Instance.audioSource, AudioManager.Instance.explosion);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    public float direction;
    public float destroyTime;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);
    }
}

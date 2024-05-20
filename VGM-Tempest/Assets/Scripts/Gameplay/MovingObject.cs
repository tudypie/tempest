using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    public float direction;

    private void Update()
    {
        transform.Translate(Vector3.forward * direction * speed * Time.deltaTime);
    }
}

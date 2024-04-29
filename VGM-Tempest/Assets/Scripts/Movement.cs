using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform[] movePoint;

    [SerializeField, Space] private Material playerMaterial;
    [SerializeField] private Material wireframeMaterial;

    [SerializeField, Space] private KeyCode leftKey;
    [SerializeField] private KeyCode rightKey;

    [SerializeField, Space] private float inputDelay = 0.05f;
    [SerializeField] private float switchSmooth = 0.1f;

    [SerializeField, Space] private int currentPointIndex;
    [SerializeField] private int lastPointIndex;

    float delay = 0;
    bool switching = false;

    private void Awake()
    {
        SetPlayerOnMovePoint();
    }

    private void Update()
    {
        movePoint[currentPointIndex].parent.GetComponent<Renderer>().material = playerMaterial;

        if (switching)
        {
            transform.position = Vector3.Lerp(transform.position, movePoint[currentPointIndex].position, switchSmooth);
            transform.rotation = Quaternion.Lerp(transform.rotation, movePoint[currentPointIndex].rotation, switchSmooth);
            Invoke(nameof(EndLerp), 0.5f);
        }

        if(Input.GetKey(rightKey))
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
                return;
            }

            lastPointIndex = currentPointIndex;
            currentPointIndex++;

            if(currentPointIndex >= movePoint.Length)
                currentPointIndex = 0;

            SetPlayerOnMovePoint();

            delay = inputDelay;
        }

        if (Input.GetKey(leftKey))
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
                return;
            }

            lastPointIndex = currentPointIndex;
            currentPointIndex--;

            if (currentPointIndex < 0)
                currentPointIndex = movePoint.Length-1;

            SetPlayerOnMovePoint();

            delay = inputDelay;
        }
    }

    private void SetPlayerOnMovePoint()
    {
        CancelInvoke();
        switching = true;
        movePoint[lastPointIndex].parent.GetComponent<Renderer>().material = wireframeMaterial;
    }

    private void EndLerp()
    {
        transform.position = movePoint[currentPointIndex].position;
        transform.rotation = movePoint[currentPointIndex].rotation;
        switching = false;
    }
}

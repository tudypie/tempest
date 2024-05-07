using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}

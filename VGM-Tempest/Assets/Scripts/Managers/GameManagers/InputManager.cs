using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Controls controls { get; private set; }

    void Awake() => controls = new();
    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();
}

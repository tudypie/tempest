using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public int playerNumber;

    [HideInInspector] public MeshRenderer playerMesh;
    public Material playerMaterial;
    public Material wireframeMaterial;

    [SerializeField, Space] private float inputDelay = 0.05f;

    [SerializeField, Space] private int currentPointIndex;
    [SerializeField] private int lastPointIndex;

    private Vector2 movementInput;
    private float delay = 0;

    public int CurrentPointIndex { get { return currentPointIndex; } }

    private float Horizontal { get; set; }

    private PlayerManager playerManager;

    private void Awake()
    {
        playerMesh = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.OnPlayerJoin(this);
        SetPlayerOnMovePoint();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        Horizontal = direction.x;
    }

    private void Update()
    {
        playerManager.movePoint[currentPointIndex].parent.GetComponent<Renderer>().material = playerMaterial;

        if (delay > 0)
        {
            delay -= Time.deltaTime;
            return;
        }

        if(Horizontal > 0)
        {
            MoveLeft();
        }
        else if (Horizontal < 0) 
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex++;

        if (currentPointIndex >= playerManager.movePoint.Length)
            currentPointIndex = 0;

        SetPlayerOnMovePoint();

        delay = inputDelay;
    }

    private void MoveRight()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex--;

        if (currentPointIndex < 0)
            currentPointIndex = playerManager.movePoint.Length - 1;

        SetPlayerOnMovePoint();

        delay = inputDelay;
    }

    private void SetPlayerOnMovePoint()
    {
        transform.position = playerManager.movePoint[currentPointIndex].position;
        transform.rotation = playerManager.movePoint[currentPointIndex].rotation;
        playerManager.movePoint[lastPointIndex].parent.GetComponent<Renderer>().material = wireframeMaterial;
    }
}

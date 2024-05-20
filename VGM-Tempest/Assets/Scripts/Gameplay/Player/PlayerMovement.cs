using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDelay = 0.05f;
    private float currentMoveDelay = 0;
    private int currentPointIndex;
    private int lastPointIndex;
    private float horizontal;

    private PlayerManager playerManager;
    private GameManager gameManager;

    public int CurrentPointIndex { get { return currentPointIndex; } }

    public void OnMove(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        horizontal = direction.x;
    }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        SetPlayerOnMovePoint();
    }

    private void Update()
    {
        gameManager.movePoint[currentPointIndex].parent.GetComponent<Renderer>().material = playerManager.playerMaterial;

        if (currentMoveDelay > 0)
        {
            currentMoveDelay -= Time.deltaTime;
            return;
        }

        if (horizontal > 0)
        {
            MoveLeft();
        }
        else if (horizontal < 0)
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex++;

        if (currentPointIndex >= gameManager.movePoint.Length)
            currentPointIndex = 0;

        SetPlayerOnMovePoint();

        currentMoveDelay = moveDelay;
    }

    private void MoveRight()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex--;

        if (currentPointIndex < 0)
            currentPointIndex = gameManager.movePoint.Length - 1;
        SetPlayerOnMovePoint();
        currentMoveDelay = moveDelay;
    }

    private void SetPlayerOnMovePoint()
    {
        transform.position = gameManager.movePoint[currentPointIndex].position;
        transform.rotation = gameManager.movePoint[currentPointIndex].rotation;
        gameManager.movePoint[lastPointIndex].parent.GetComponent<Renderer>().material = gameManager.wireframeMaterial;
    }
}

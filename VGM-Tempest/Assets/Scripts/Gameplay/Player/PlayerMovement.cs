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
        SetPlayerOnWireframeLine();
    }

    private void Update()
    {
        gameManager.SetLineColor(currentPointIndex, playerManager.playerNumber);

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

    private void MoveRight()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex++;

        if (currentPointIndex >= gameManager.wireframeLine.Length)
            currentPointIndex = 0;

        SetPlayerOnWireframeLine();

        currentMoveDelay = moveDelay;
    }

    private void MoveLeft()
    {
        lastPointIndex = currentPointIndex;
        currentPointIndex--;

        if (currentPointIndex < 0)
            currentPointIndex = gameManager.wireframeLine.Length - 1;
        SetPlayerOnWireframeLine();
        currentMoveDelay = moveDelay;
    }

    private void SetPlayerOnWireframeLine()
    {
        transform.position = gameManager.wireframeLine[currentPointIndex].position;
        transform.rotation = gameManager.wireframeLine[currentPointIndex].rotation;
        gameManager.wireframeLine[lastPointIndex].parent.GetComponent<Renderer>().material = gameManager.wireframeMaterial;
    }
}

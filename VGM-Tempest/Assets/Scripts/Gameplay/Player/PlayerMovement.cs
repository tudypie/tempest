using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDelay = 0.05f;
    private float currentMoveDelay;
    public int currentLine;
    private int lastLine;
    private float horizontal;

    private PlayerManager playerManager;
    private GameManager gameManager;

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
        if (!gameManager.ongoingLevel) return;

        gameManager.SetPlayerLineColor(currentLine, playerManager.playerNumber);

        if (currentMoveDelay > 0)
        {
            currentMoveDelay -= Time.deltaTime;
            return;
        }

        if(playerManager.playerNumber == 0)
            horizontal = InputManager.controls.Player1.Move.ReadValue<Vector2>().x;
        else
            horizontal = InputManager.controls.Player2.Move.ReadValue<Vector2>().x;

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
        lastLine = currentLine;
        currentLine++;

        if (currentLine >= gameManager.wireframe.lines.Length)
            currentLine = 0;

        SetPlayerOnWireframeLine();

        currentMoveDelay = moveDelay;
    }

    private void MoveLeft()
    {
        lastLine = currentLine;
        currentLine--;

        if (currentLine < 0)
            currentLine = gameManager.wireframe.lines.Length - 1;
        SetPlayerOnWireframeLine();
        currentMoveDelay = moveDelay;
    }

    public void SetPlayerOnWireframeLine()
    {
        transform.position = Wireframe.Instance.lines[currentLine].position;
        transform.rotation = Wireframe.Instance.lines[currentLine].rotation;
        Wireframe.Instance.ResetLineMaterial(lastLine);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDelay = 0.15f; // Increased slightly for better control
    private float currentMoveDelay;
    public int currentLine;
    private int lastLine;

    // Tracking button states
    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private PlayerManager playerManager;
    private GameManager gameManager;

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

        // Handle the delay timer
        if (currentMoveDelay > 0)
        {
            currentMoveDelay -= Time.deltaTime;
        }

        // Only move if the timer has reached zero
        if (currentMoveDelay <= 0)
        {
            // Check Keyboard AND UI Buttons
            float keyboardHorizontal = 0;
            if (playerManager.playerNumber == 0)
                keyboardHorizontal = InputManager.controls.Player1.Move.ReadValue<Vector2>().x;
            else
                keyboardHorizontal = InputManager.controls.Player2.Move.ReadValue<Vector2>().x;

            // Move logic
            if (isMovingLeft || keyboardHorizontal > 0.1f)
            {
                MoveLeft();
            }
            else if (isMovingRight || keyboardHorizontal < -0.1f)
            {
                MoveRight();
            }
        }
    }

    // --- UI BUTTON EVENTS ---
    public void PointerDownLeft() => isMovingLeft = true;
    public void PointerUpLeft() => isMovingLeft = false;

    public void PointerDownRight() => isMovingRight = true;
    public void PointerUpRight() => isMovingRight = false;

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
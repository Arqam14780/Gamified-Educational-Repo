using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance { get; private set; }

    [Header("Input")]
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool SprintPressed { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetMoveInput(Vector2 input)
    {
        MoveInput = Vector2.ClampMagnitude(input, 1f);
    }

    public void SetLookInput(Vector2 input)
    {
        LookInput = input;
    }

    public void PressJump()
    {
        JumpPressed = true;
        Invoke("JumpDelay", 0.2f);
    }

    private void JumpDelay()
    {
        JumpPressed = false;
    }

    public void PressSprint()
    {
        SprintPressed = true;
    }
    public void ReleaseSprint()
    {
        SprintPressed = false;
    }

    public void ResetInput()
    {
        MoveInput = Vector2.zero;
        LookInput = Vector2.zero;
        JumpPressed = false;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-2)]
public class PlayerLocomotionInput : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    #region Class Variables
    public Vector3 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Quaternion LookRotation { get; private set; }
    private Matrix4x4 skewMatrix;
    public bool TimeWarpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool ResetScenePressed { get; private set; }
    public bool ResetGamePressed { get; private set; }
    #endregion

    #region Startup
    private void Awake()
    {
        skewMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    }

    private void OnEnable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.LogError("Player controls is not initialized - cannot enable");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.Player.Enable();
        PlayerInputManager.Instance.PlayerControls.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        if (PlayerInputManager.Instance?.PlayerControls == null)
        {
            Debug.LogError("Player controls is not initialized - cannot disable");
            return;
        }

        PlayerInputManager.Instance.PlayerControls.Player.Disable();
        PlayerInputManager.Instance.PlayerControls.Player.RemoveCallbacks(this);
    }
    #endregion

    #region Late Update Logic
    private void LateUpdate()
    {
        TimeWarpPressed = false;
        InteractPressed = false;
        ResetScenePressed = false;
        ResetGamePressed = false;
    }
    #endregion

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = skewMatrix.MultiplyPoint3x4(new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y).normalized);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //var direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y) + transform.position;
        //direction = direction - transform.position;
        //LookRotation = Quaternion.LookRotation(direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        InteractPressed = true;
    }

    public void OnTimeWarp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        TimeWarpPressed = true;
    }

    public void OnResetScene(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        ResetScenePressed = true;
    }

    public void OnResetGame(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        ResetGamePressed = true;
    }
    #endregion
}
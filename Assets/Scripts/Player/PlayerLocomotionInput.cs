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
    }
    #endregion

    #region Input Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = skewMatrix.MultiplyPoint3x4(new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y).normalized);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y) + transform.position;
        direction = direction - transform.position;
        LookRotation = Quaternion.LookRotation(direction);
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        Vector3 aimPoint = Vector3.zero;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, ~6))
        {
            Vector3 playerHeight = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
            Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            float length = Vector3.Distance(playerHeight, hitPoint);

            var rad = 45 * Mathf.Deg2Rad;

            float hypotenuse = length / (Mathf.Sin(rad));
            float distanceFromCamera = hit.distance;

            if (this.transform.position.y > hit.point.y)
            {
                aimPoint = castPoint.GetPoint(distanceFromCamera - hypotenuse);
            }
            else if (this.transform.position.y < hit.point.y)
            {
                aimPoint = castPoint.GetPoint(distanceFromCamera + hypotenuse);
            }
            else
                aimPoint = castPoint.GetPoint(distanceFromCamera);
        }

        var direction = aimPoint - transform.position;
        direction.y = 0;
        LookRotation = Quaternion.LookRotation(direction);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnTimeWarp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        TimeWarpPressed = true;
    }
    #endregion
}
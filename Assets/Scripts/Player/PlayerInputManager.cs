using UnityEngine;

[DefaultExecutionOrder(-3)]
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public InputSystem_Actions PlayerControls { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        PlayerControls = new InputSystem_Actions();
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }
}

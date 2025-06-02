using UnityEngine;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private bool _inPast = true;
    private Vector3 _input;
    private PlayerLocomotionInput _playerLocomotionInput;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        _input = _playerLocomotionInput.MovementInput;
        Look();
        if (_playerLocomotionInput.TimeWarpPressed)
        {
            TimeWarp();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(_input, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _rigidBody.MovePosition(transform.position + _input * _input.normalized.magnitude * _speed * Time.deltaTime);
    }

    private void TimeWarp()
    {
        float yPosition;
        if (_inPast)
            yPosition = -500f;
        else yPosition = 0f;

        _rigidBody.Sleep();
        this.gameObject.SetActive(false);
        this.transform.position = new Vector3(this.transform.position.x, yPosition, this.transform.position.z);
        this.gameObject.SetActive(true);
        _rigidBody.WakeUp();

        _inPast = !_inPast;
    }
}
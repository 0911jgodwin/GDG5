using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private TimeWarp _transitionManager;
    [SerializeField] public AnimationCurve _shakeIntensity;
    private Vector3 _input;
    private PlayerLocomotionInput _playerLocomotionInput;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        if (_transitionManager._transitionActive)
            return;
        _input = _playerLocomotionInput.MovementInput;
        Look();
        if (_playerLocomotionInput.TimeWarpPressed)
        {
            TimeWarp();
        }
    }

    private void FixedUpdate()
    {
        if (_transitionManager._transitionActive)
            return;
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
        float yPosition = _transitionManager._inPast ? -500f : 0f;
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(this.transform.position.x, yPosition, this.transform.position.z), 1f);
        if (!(hitColliders.Length > 0))
            _transitionManager.StartTransition();
        else
            StartCoroutine(ScreenShake());

        
    }

    IEnumerator ScreenShake()
    {
        Vector3 startPosition = _cameraPivot.position;
        float elapsedTime = 0f;
        float duration = 0.6f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = _shakeIntensity.Evaluate(elapsedTime / duration);
            _cameraPivot.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        _cameraPivot.position = startPosition;
    }
}
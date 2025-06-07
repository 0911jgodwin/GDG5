using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

[DefaultExecutionOrder(-1)]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private TimeWarp _transitionManager;
    [SerializeField] private FadeInOut _fadeManager;
    [SerializeField] public AnimationCurve _shakeIntensity;
    [SerializeField] private bool _isDragging;
    [SerializeField] private bool _hasKey = false;
    private bool _lerpingToPosition;
    private GameObject _draggableObject;
    private Vector3 _input;
    private PlayerLocomotionInput _playerLocomotionInput;
    [SerializeField] private GameObject _nearestInteractable;

    private void Awake()
    {
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void Update()
    {
        if (_transitionManager._transitionActive || _lerpingToPosition)
            return;

        _input = _playerLocomotionInput.MovementInput;

        if (!_isDragging)
            Look();

        if (_playerLocomotionInput.TimeWarpPressed)
            TimeWarp();

        if (_playerLocomotionInput.InteractPressed && _nearestInteractable != null)
            Interact();
    }

    private void FixedUpdate()
    {
        if (_transitionManager._transitionActive || _lerpingToPosition)
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

    private void Interact()
    {
        if (_nearestInteractable.tag == "PlayButton" )
        {
            _fadeManager.PlayFade();
            return;
        }
        if (_isDragging)
        {
            _draggableObject.transform.parent = null;
            _draggableObject = null;
            _isDragging = false;
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            return;
        }

        StartCoroutine(SlideToPosition());

        
    }

    private void StartDragging(GameObject draggable)
    {
        _isDragging = true;
        _draggableObject = draggable;
        _draggableObject.transform.parent = this.transform;
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void TimeWarp()
    {
        if (_isDragging) { 
            StartCoroutine(ScreenShake());
            return;
        }

        float yOffset = _transitionManager._inPast ? -500f : 500f;
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(this.transform.position.x, this.transform.position.y + yOffset, this.transform.position.z), 0.5f);
        bool collided = false;
        if ((hitColliders.Length > 0))
        {
            foreach (Collider collider in hitColliders)
            {
                if (!(collider.gameObject.layer == 2))
                    collided = true;
            }
        }

        if (!collided)
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

    IEnumerator SlideToPosition()
    {
        _lerpingToPosition = true;
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        Vector3 startPos = this.transform.position;
        Quaternion startRot = this.transform.rotation;
        Interactable interactedObject = _nearestInteractable.GetComponent<Interactable>();
        Vector3 destinationPos;
        if (Vector3.Distance(interactedObject._latchPoints[0].transform.position, startPos) < Vector3.Distance(interactedObject._latchPoints[1].transform.position, startPos))
        {
            destinationPos = interactedObject._latchPoints[0].transform.position;
        } else
        {
            destinationPos = interactedObject._latchPoints[1].transform.position;
        }

        destinationPos.y = startPos.y;

        Vector3 relativePos = interactedObject._latchPoints[0].transform.position;
        relativePos.x = relativePos.x + 1.2f;
        relativePos.y = startPos.y;
        relativePos = relativePos - startPos;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion destinationRot = Quaternion.LookRotation(relativePos, Vector3.up);

        
        float rate = 5f;
        while (Time.time - startTime <= 1 / rate)
        { // until one second passed
            transform.position = Vector3.Lerp(startPos, destinationPos, (Time.time - startTime) * rate); // lerp from A to B in one second
            transform.rotation = Quaternion.Lerp(startRot, destinationRot, (Time.time - startTime) * rate);
            yield return 1; // wait for next frame
        }
        StartDragging(_nearestInteractable);
        _lerpingToPosition = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable" || other.tag == "PlayButton")
            _nearestInteractable = other.gameObject;
        else if (other.tag == "Key")
        {
            Destroy(other.gameObject);
            _hasKey = true;
        }
        else if (other.tag == "Door")
        {
            _fadeManager.PlayFade();
            //Check if door needs key, otherwise finish level
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
            _nearestInteractable = null;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;
    void Start()
    {
        _doorAnimator = GetComponent<Animator>();
        if (this.CompareTag("Door"))
            _doorAnimator.SetTrigger("Open");
    }

    public void OpenDoor()
    {
        _doorAnimator.SetTrigger("Open");
    }

    public void ChangeTag()
    {
        this.tag = "Door";
    }
}

using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] GameObject _player;
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] public bool _inPast = true;
    [SerializeField] public bool _transitionActive = false;

    public void StartTransition()
    {
        _transitionActive = true;
        _transitionAnimator.SetTrigger("TimeWarp");
    }


    private void ChangeScene()
    {
        float yPosition;
        if (_inPast)
            yPosition = -500f;
        else yPosition = 0f;

        // switches the music
        FindObjectOfType<MusicManager>().SwitchMusic();

        _player.GetComponent<Rigidbody>().Sleep();
        _player.SetActive(false);
        _player.transform.position = new Vector3(_player.transform.position.x, yPosition, _player.transform.position.z);
        _player.SetActive(true);
        _player.GetComponent<Rigidbody>().WakeUp();

        _cameraPivot.position = new Vector3(0f, yPosition, 0f);

        _inPast = !_inPast;


    }

    private void FinishedTransition()
    {
        _transitionActive = false;
    }
}

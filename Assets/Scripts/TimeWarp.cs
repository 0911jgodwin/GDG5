using UnityEngine;

public class TimeWarp : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] GameObject _player;
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] public bool _inPast = true;
    [SerializeField] public bool _transitionActive = false;

    public void Awake()
    {
        if (FindFirstObjectByType<MusicManager>().inPast == !_inPast)
        {
            FindFirstObjectByType<MusicManager>().SwitchMusic();
        }
    }
    public void StartTransition()
    {
        // switches the music
        FindFirstObjectByType<MusicManager>().SwitchMusic();

        _transitionActive = true;
        _transitionAnimator.SetTrigger("TimeWarp");
    }


    private void ChangeScene()
    {
        float yOffset;
        if (_inPast)
            yOffset = -500f;
        else yOffset = 500f;



        _player.GetComponent<Rigidbody>().Sleep();
        _player.SetActive(false);
        _player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + yOffset, _player.transform.position.z);
        _player.SetActive(true);
        _player.GetComponent<Rigidbody>().WakeUp();

        _cameraPivot.position = new Vector3(_cameraPivot.position.x, _cameraPivot.position.y +yOffset, _cameraPivot.position.z);

        _inPast = !_inPast;


    }

    private void FinishedTransition()
    {
        _transitionActive = false;
    }
}

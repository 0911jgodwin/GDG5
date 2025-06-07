using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] private Animator _fadeManager;

    public void PlayFade()
    {
        _fadeManager.SetTrigger("FadeOut");
    }

    public void FadeOut()
    {
        _player.GetComponent<PlayerController>().NextScene();
    }
}

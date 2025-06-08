using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource trackA;
    public AudioSource trackB;

    public AudioSource movementAudioSource;
    public AudioSource sfxSource; 
    public AudioClip transitionSFX;

    public float fadeDuration = 1.0f;
    public float movementVolume = 0.5f;

    private AudioSource activeTrack;
    private AudioSource inactiveTrack;
    private bool isSwitching = false;
    [SerializeField] public bool inPast = true;

    void Awake()
    {
        // Singleton check
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Start both tracks, one at full volume, one muted
        trackA.volume = 1.0f;
        trackB.volume = 0.0f;

        trackA.Play();
        trackB.Play();

        activeTrack = trackA;
        inactiveTrack = trackB;
    }

    public void SwitchMusic()
    {
        if (!isSwitching)
            StartCoroutine(CrossfadeTracks());
    }

    private System.Collections.IEnumerator CrossfadeTracks()
    {
        isSwitching = true;
        inPast = !inPast;
        // Play transition SFX
        if (sfxSource != null && transitionSFX != null)
        {
            sfxSource.PlayOneShot(transitionSFX);
        }

        float timer = 0f;
        float startVolumeActive = activeTrack.volume;
        float startVolumeInactive = inactiveTrack.volume;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            activeTrack.volume = Mathf.Lerp(startVolumeActive, 0f, t);
            inactiveTrack.volume = Mathf.Lerp(startVolumeInactive, 1f, t);

            yield return null;
        }

        // Swap the roles
        AudioSource temp = activeTrack;
        activeTrack = inactiveTrack;
        inactiveTrack = temp;

        isSwitching = false;
    }

}

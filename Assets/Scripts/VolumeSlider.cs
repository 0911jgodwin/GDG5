using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    private MusicManager musicManager;

    private void Awake()
    {
        musicManager = FindFirstObjectByType<MusicManager>();
    }

    private void Update()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, startPosition.position.x, endPosition.position.x);
        clampedPosition.z = startPosition.position.z;
        this.transform.position = clampedPosition;
        if (musicManager != null)
            musicManager.SetMaxVolume(CalculateVolume());
    }

    private float CalculateVolume()
    {
        float distance = Vector3.Distance(startPosition.position, endPosition.position);
        float currentPosition = Vector3.Distance(startPosition.position, this.transform.position);
        return currentPosition / distance;
    }
}

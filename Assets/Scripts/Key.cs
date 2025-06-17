using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private GameObject lockedDoor;
    public void Open()
    {
        lockedDoor.GetComponent<Door>().OpenDoor();
    }
}

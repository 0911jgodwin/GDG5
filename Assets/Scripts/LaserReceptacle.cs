using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LaserReceptacle : MonoBehaviour
{
    private bool isOpen = false;
    [SerializeField] private GameObject lockedDoor;
    public void Open()
    {
        if (!isOpen)
        {
            lockedDoor.GetComponent<Door>().OpenDoor();
            isOpen = true;
        }
    }
}

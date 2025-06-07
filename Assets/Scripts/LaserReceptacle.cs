using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class LaserReceptacle : MonoBehaviour
{
    private bool isOpen = false;
    public void Open()
    {
        if (!isOpen)
        {
            Debug.Log("TriggerOpen");
            isOpen = true;
        }
    }
}

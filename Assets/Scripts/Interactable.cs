using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject _tetheredObject;
    [SerializeField] public GameObject[] _latchPoints;
    private void Update()
    {
        _tetheredObject.transform.position = new Vector3(this.transform.position.x, _tetheredObject.transform.position.y, this.transform.position.z);
    }
}

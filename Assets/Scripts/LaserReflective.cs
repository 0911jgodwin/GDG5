using UnityEngine;

public class LaserReflective : MonoBehaviour
{
    public void Reflect(Laser laser, Ray incomingRay, RaycastHit hitInfo)
    {
        var outgoingDirection = Vector3.Reflect(incomingRay.direction, hitInfo.normal);
        laser.CastBeam(hitInfo.point, outgoingDirection);
    }
}

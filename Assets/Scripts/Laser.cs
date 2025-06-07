using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Laser : MonoBehaviour
{
    bool activated = true;

    LineRenderer _lineRenderer;
    [SerializeField] LaserRendererSettings _laserRendererSettings;

    Vector3 sourcePosition;
    const float _farDistance = 100f;
    List<Vector3> bouncePositions;
    int _maxBounces = 5;

    private void Awake()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _laserRendererSettings.Apply(_lineRenderer);


    }

    private void FixedUpdate()
    {
        if (!activated)
        {
            _lineRenderer.positionCount = 0;
            return;
        }

        sourcePosition = transform.position + transform.forward * 0.2501f;
        bouncePositions = new List<Vector3>() { sourcePosition };

        CastBeam(sourcePosition, transform.forward);

        _lineRenderer.positionCount = bouncePositions.Count;
        _lineRenderer.SetPositions(bouncePositions.ToArray());
    }

    public void CastBeam(Vector3 origin, Vector3 direction)
    {
        if (bouncePositions.Count > _maxBounces) return;

        var ray = new Ray(origin, direction);
        bool didHit = Physics.Raycast(ray, out RaycastHit hitInfo, _farDistance);

        if (!didHit)
        {
            var endPoint = origin + direction * _farDistance;
            bouncePositions.Add(endPoint);
            return;
        }
        bouncePositions.Add(hitInfo.point);

        var _reflectiveObject = hitInfo.collider.GetComponent<LaserReflective>();
        if (_reflectiveObject != null) {
            _reflectiveObject.Reflect(this, ray, hitInfo);
        }

        var _recptacleObject = hitInfo.collider.GetComponent<LaserReceptacle>();
        if (_recptacleObject != null)
        {
            _recptacleObject.Open();
        }
    }
}

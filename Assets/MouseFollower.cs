using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;

    [SerializeField]
    private Camera targetCamera;

    [SerializeField]
    private bool rotateTowardMouse;

    [SerializeField]
    private float rotationOffsetDegrees;

    private Vector3 anchorLocalPosition;

    private void Awake()
    {
        CacheAnchor();
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        CacheAnchor();
    }

    private void OnValidate()
    {
        radius = Mathf.Max(0f, radius);
    }

    private void Update()
    {
        if (targetCamera == null)
        {
            return;
        }

        Vector3 anchorWorld = GetAnchorWorldPosition();
        Vector3 anchorScreen = targetCamera.WorldToScreenPoint(anchorWorld);
        Vector3 mouseScreen = Input.mousePosition;
        mouseScreen.z = anchorScreen.z;
        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);

        Vector3 offset = mouseWorld - anchorWorld;
        float maxDistance = radius;
        float offsetSqrMagnitude = offset.sqrMagnitude;
        if (offsetSqrMagnitude > maxDistance * maxDistance && offsetSqrMagnitude > 0f)
        {
            offset = offset.normalized * maxDistance;
        }

        Vector3 desiredWorldPosition = anchorWorld + offset;
        Vector3 desiredLocalPosition = transform.parent != null
            ? transform.parent.InverseTransformPoint(desiredWorldPosition)
            : desiredWorldPosition;

        transform.localPosition = desiredLocalPosition;

        if (rotateTowardMouse)
        {
            RotateToward(targetCamera, anchorWorld, desiredWorldPosition);
        }
    }

    private void RotateToward(Camera camera, Vector3 anchorWorld, Vector3 targetWorld)
    {
        Vector3 direction = targetWorld - anchorWorld;
        direction.z = 0f;
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + rotationOffsetDegrees;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private Vector3 GetAnchorWorldPosition()
    {
        if (transform.parent != null)
        {
            return transform.parent.TransformPoint(anchorLocalPosition);
        }

        return anchorLocalPosition;
    }

    private void CacheAnchor()
    {
        anchorLocalPosition = transform.localPosition;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Repels the hardware cursor whenever it touches the shield's edge collider.
/// </summary>
[RequireComponent(typeof(EdgeCollider2D))]
public class ShieldBubbleRepel : MonoBehaviour
{
    [Tooltip("Collider that defines the shield boundary.")]
    [SerializeField] EdgeCollider2D shieldCollider;

    [Tooltip("World-space distance from the edge considered as contact.")]
    [SerializeField] float contactBuffer = 0.05f;

    [Tooltip("How far past the edge (world units) the cursor gets teleported.")]
    [SerializeField] float boundaryOffset = 0.02f;

    [Tooltip("Optional override; defaults to Camera.main when left empty.")]
    [SerializeField] Camera targetCamera;

    Vector2 lastWorldCursor;
    bool hasLastWorldCursor;

    void Reset()
    {
        shieldCollider = GetComponent<EdgeCollider2D>();
    }

    void OnValidate()
    {
        if (shieldCollider == null)
            shieldCollider = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        Mouse mouse = Mouse.current;

        if (cam == null || mouse == null || shieldCollider == null || !Application.isFocused)
            return;

        Vector2 cursor = mouse.position.ReadValue();
        float depth = Mathf.Abs(transform.position.z - cam.transform.position.z);
        Vector3 worldPoint3 = cam.ScreenToWorldPoint(new Vector3(cursor.x, cursor.y, depth));
        Vector2 mouseWorld = worldPoint3;

        Vector2 closest = shieldCollider.ClosestPoint(mouseWorld);
        float distance = Vector2.Distance(mouseWorld, closest);

        bool insideBoundary = distance <= contactBuffer;
        Vector2 intersectionWorld = Vector2.one;
        bool crossedBoundary = hasLastWorldCursor && TryGetIntersection(lastWorldCursor, mouseWorld, out intersectionWorld);


        if (!insideBoundary && !crossedBoundary)
        {
            lastWorldCursor = mouseWorld;
            hasLastWorldCursor = true;
            return;
        }


        Vector2 clampPoint = insideBoundary ? closest : intersectionWorld;
        Vector2 worldDir = clampPoint - (Vector2)transform.position;
        if (worldDir.sqrMagnitude < 0.0001f)
        {
            worldDir = Vector2.up;
        }
        worldDir.Normalize();

        Vector2 boundaryWorld = clampPoint + worldDir * Mathf.Max(boundaryOffset, 0.0001f);
        Vector3 boundaryScreen3 = cam.WorldToScreenPoint(new Vector3(boundaryWorld.x, boundaryWorld.y, worldPoint3.z));
        Vector2 boundaryScreen = boundaryScreen3;

        mouse.WarpCursorPosition(boundaryScreen);
        InputState.Change(mouse.position, boundaryScreen);

        lastWorldCursor = boundaryWorld;
        hasLastWorldCursor = true;
    }

    bool TryGetIntersection(Vector2 start, Vector2 end, out Vector2 hit)
    {
        hit = default;
        Vector2[] points = shieldCollider.points;
        if (points == null || points.Length < 2)
            return false;

        Vector2 offset = shieldCollider.offset;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 a = (Vector2)shieldCollider.transform.TransformPoint(points[i] + offset);
            Vector2 b = (Vector2)shieldCollider.transform.TransformPoint(points[i + 1] + offset);

            if (SegmentsIntersect(start, end, a, b, out Vector2 intersection))
            {
                hit = intersection;
                return true;
            }
        }

        return false;
    }

    static bool SegmentsIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = default;
        Vector2 r = p2 - p1;
        Vector2 s = p4 - p3;
        float denom = r.x * s.y - r.y * s.x;

        if (Mathf.Approximately(denom, 0f))
            return false;

        Vector2 diff = p3 - p1;
        float u = (diff.x * r.y - diff.y * r.x) / denom;
        float t = (diff.x * s.y - diff.y * s.x) / denom;

        if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
        {
            intersection = p1 + t * r;
            return true;
        }

        return false;
    }
}

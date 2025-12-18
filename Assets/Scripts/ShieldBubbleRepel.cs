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

    [Tooltip("How much world offset is used to derive the push direction.")]
    [SerializeField] float pushDirectionNudge = 0.05f;

    [Tooltip("How fast (pixels/second) the cursor is pushed once in contact.")]
    [SerializeField] float pushStrength = 250f;

    [Tooltip("Optional override; defaults to Camera.main when left empty.")]
    [SerializeField] Camera targetCamera;

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

        if (distance > contactBuffer)
            return;

        Vector2 worldDir = mouseWorld - closest;
        if (worldDir.sqrMagnitude < 0.0001f)
        {
            worldDir = mouseWorld - (Vector2)shieldCollider.bounds.center;
            if (worldDir.sqrMagnitude < 0.0001f)
                worldDir = Vector2.right;
        }
        worldDir.Normalize();

        Vector3 nudgeWorld = (Vector3)(mouseWorld + worldDir * pushDirectionNudge);
        Vector2 pushDirScreen = (Vector2)cam.WorldToScreenPoint(nudgeWorld) - cursor;
        if (pushDirScreen.sqrMagnitude < 0.0001f)
            pushDirScreen = worldDir;
        pushDirScreen.Normalize();

        Vector2 newPos = cursor + pushDirScreen * pushStrength * Time.deltaTime;

        mouse.WarpCursorPosition(newPos);
        InputState.Change(mouse.position, newPos);
    }
}

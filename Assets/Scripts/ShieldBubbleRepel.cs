using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

/// <summary>
/// Repels the hardware cursor from the shield bubble using the Input System cursor warp API.
/// </summary>
public class ShieldBubbleRepel : MonoBehaviour
{
    [Tooltip("Radius in screen pixels at which the cursor begins getting pushed away.")]
    [SerializeField] float repelRadius = 80f;

    [Tooltip("How fast (pixels/second) the cursor is pushed once inside the radius.")]
    [SerializeField] float pushStrength = 250f;

    [Tooltip("Optional override; defaults to Camera.main when left empty.")]
    [SerializeField] Camera targetCamera;

    void Update()
    {
        Camera cam = targetCamera != null ? targetCamera : Camera.main;
        Mouse mouse = Mouse.current;

        if (cam == null || mouse == null || !Application.isFocused)
            return;

        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        if (screenPoint.z < 0f)
            return;

        Vector2 cursor = mouse.position.ReadValue();
        Vector2 toCursor = cursor - (Vector2)screenPoint;

        if (toCursor.sqrMagnitude > repelRadius * repelRadius)
            return;

        Vector2 pushDir = toCursor.sqrMagnitude < 0.001f ? Random.insideUnitCircle.normalized : toCursor.normalized;
        Vector2 newPos = cursor + pushDir * pushStrength * Time.deltaTime;

        mouse.WarpCursorPosition(newPos);
        InputState.Change(mouse.position, newPos); // keep Input System events in sync
    }
}

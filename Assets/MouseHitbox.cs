using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MouseHitbox : MonoBehaviour
{
    [SerializeField]
    private float stunDuration = 1f;

    [SerializeField]
    private float immunityDuration = 1f;

    private bool isStunned;
    private bool isImmune;
    private Coroutine stunRoutine;
    private Vector2 frozenMouseScreenPosition;
    private bool hasFrozenMousePosition;

    public bool IsStunned => isStunned;
    public bool IsImmune => isImmune;

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            FreezeMouseCursor();
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Set this to be the distance from the camera to the object
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;
    }

    public bool TryApplyArrowHit()
    {
        if (isStunned || isImmune)
        {
            return false;
        }

        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
        }

        if(GlobalController.Instance.ArrowImmune)
        {
            return false;
        }

        Vector3 currentMouse = Input.mousePosition;
        frozenMouseScreenPosition = new Vector2(currentMouse.x, currentMouse.y);
        hasFrozenMousePosition = true;
        FreezeMouseCursor();

        stunRoutine = StartCoroutine(StunRoutine());
        return true;
    }

    private IEnumerator StunRoutine()
    {
        isStunned = true;
        float stunTime = Mathf.Max(stunDuration, 0f);
        if (stunTime > 0f)
        {
            yield return new WaitForSeconds(stunTime);
        }
        else
        {
            yield return null;
        }

        isStunned = false;
        hasFrozenMousePosition = false;

        isImmune = true;
        float immunityTime = Mathf.Max(immunityDuration, 0f);
        if (immunityTime > 0f)
        {
            yield return new WaitForSeconds(immunityTime);
        }
        else
        {
            yield return null;
        }

        isImmune = false;
        stunRoutine = null;
    }

    private void OnDisable()
    {
        if (stunRoutine != null)
        {
            StopCoroutine(stunRoutine);
            stunRoutine = null;
        }

        isStunned = false;
        isImmune = false;
        hasFrozenMousePosition = false;
    }

    private void FreezeMouseCursor()
    {
        if (!hasFrozenMousePosition)
        {
            Vector3 currentMouse = Input.mousePosition;
            frozenMouseScreenPosition = new Vector2(currentMouse.x, currentMouse.y);
            hasFrozenMousePosition = true;
        }

        if (!Application.isFocused)
        {
            return;
        }

        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;
        }

        mouse.WarpCursorPosition(frozenMouseScreenPosition);
        InputState.Change(mouse.position, frozenMouseScreenPosition);
    }
}

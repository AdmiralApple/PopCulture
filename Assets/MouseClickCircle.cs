using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MouseClickCircle : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;

    private CircleCollider2D circleCollider;
    private readonly HashSet<Bubble> trackedBubbles = new HashSet<Bubble>();
    private readonly List<Bubble> bubbleBuffer = new List<Bubble>();

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.isTrigger = true;
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();

        bool autoPopEnabled = GlobalController.Instance != null && GlobalController.Instance.AutoPop;
        if (autoPopEnabled || Input.GetMouseButtonDown(0))
        {
            PopBubblesInRange();
        }
    }

    private void OnDisable()
    {
        ClearTrackedBubbles();
    }

    private void UpdatePosition()
    {
        if (targetCamera == null)
        {
            return;
        }

        Vector3 mouseScreen = Input.mousePosition;
        float depth = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);
        mouseScreen.z = depth;

        Vector3 mouseWorld = targetCamera.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = transform.position.z;
        transform.position = mouseWorld;
    }

    private void PopBubblesInRange()
    {
        if (trackedBubbles.Count == 0)
        {
            return;
        }

        bubbleBuffer.Clear();
        bubbleBuffer.AddRange(trackedBubbles);

        foreach (Bubble bubble in bubbleBuffer)
        {
            if (bubble == null || bubble.Popped)
            {
                StopTrackingBubble(bubble);
                continue;
            }

            bubble.Pop(new PopData(PopType.Mouse));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bubble bubble = other.GetComponent<Bubble>();
        if (bubble == null || bubble.Popped)
        {
            return;
        }

        if (trackedBubbles.Add(bubble))
        {
            bubble.OnBubblePopped += HandleBubbleFinished;
            bubble.OnBubbleDestroyed += HandleBubbleFinished;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Bubble bubble = other.GetComponent<Bubble>();
        if (bubble == null)
        {
            return;
        }

        StopTrackingBubble(bubble);
    }

    private void HandleBubbleFinished(Bubble bubble)
    {
        StopTrackingBubble(bubble);
    }

    private void StopTrackingBubble(Bubble bubble)
    {
        if (bubble == null)
        {
            return;
        }

        bubble.OnBubblePopped -= HandleBubbleFinished;
        bubble.OnBubbleDestroyed -= HandleBubbleFinished;
        trackedBubbles.Remove(bubble);
    }

    private void ClearTrackedBubbles()
    {
        bubbleBuffer.Clear();
        bubbleBuffer.AddRange(trackedBubbles);
        foreach (Bubble bubble in bubbleBuffer)
        {
            StopTrackingBubble(bubble);
        }

        trackedBubbles.Clear();
        bubbleBuffer.Clear();
    }
}

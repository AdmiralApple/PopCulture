using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform SpawnBottom;
    public Transform SpawnTop;
    public GameObject BubblePrefab;

    [Header("Row Rules")]
    public int maxBubblesPerRow = 10;
    public float extraHorizontalGap = 0.1f;     // extra space between rows (world units)
    public float extraVerticalGap = 0.05f;       // extra space between bubbles in same row

    [Header("Fallback timing")]
    public float minWait = 0.01f; // prevents divide-by-zero / super tiny waits

    float bubbleDiameterX;
    float bubbleDiameterY;

    public float spawnTime = 1.0f;

    void Awake()
    {
        // Measure prefab size (SpriteRenderer preferred, fallback to Collider2D).
        var sr = BubblePrefab.GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            bubbleDiameterX = sr.bounds.size.x;
            bubbleDiameterY = sr.bounds.size.y;
        }
        else
        {
            var col = BubblePrefab.GetComponentInChildren<Collider2D>();
            if (col != null)
            {
                bubbleDiameterX = col.bounds.size.x;
                bubbleDiameterY = col.bounds.size.y;
            }
            else
            {
                // If neither exists, pick a sane default so you notice it’s wrong.
                bubbleDiameterX = 0.5f;
                bubbleDiameterY = 0.5f;
            }
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            SpawnRow();

            float speed = Mathf.Max(0.0001f, GlobalController.Instance.WrapperSpeed);
            float dx = bubbleDiameterX + extraHorizontalGap;
            float wait = Mathf.Max(minWait, dx / speed);

            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnRow()
    {
        float minY = SpawnBottom.position.y;
        float maxY = SpawnTop.position.y;

        // Minimum vertical spacing so bubbles in the same row don't overlap.
        float minYSpacing = bubbleDiameterY + extraVerticalGap;

        // Try to pick up to maxBubblesPerRow Y positions without overlaps.
        List<float> ys = new List<float>(maxBubblesPerRow);
        int attempts = 0;

        while (ys.Count < maxBubblesPerRow && attempts < 200)
        {
            attempts++;
            float y = Random.Range(minY, maxY);

            bool ok = true;
            for (int i = 0; i < ys.Count; i++)
            {
                if (Mathf.Abs(ys[i] - y) < minYSpacing)
                {
                    ok = false;
                    break;
                }
            }

            if (ok) ys.Add(y);
        }

        // Spawn them all at the same X (same "frame/row")
        for (int i = 0; i < ys.Count; i++)
        {
            Vector3 pos = new Vector3(SpawnTop.position.x, ys[i], SpawnTop.position.z);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(BubblePrefab, pos, rot);
        }
    }
}

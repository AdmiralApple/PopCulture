using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject BubblePrefab;

    public Transform SpawnFirstPoint;

    [Header("Row Rules")]
    public int maxBubblesPerRow = 8;
    public float HorizontalGap = 0.1f;     // extra space between rows (world units)
    float bubbleDiameter = 1;
    public float SpawnChance = .02f;

    [Header("Fallback timing")]
    public float minWait = 0.01f; // prevents divide-by-zero / super tiny waits



    private bool Frame1or2 = true;


    IEnumerator Start()
    {
        while (true)
        {
            SpawnRow();

            float speed = Mathf.Max(0.0001f, GlobalController.Instance.WrapperSpeed);
            float dx = bubbleDiameter + HorizontalGap;
            float wait = Mathf.Max(minWait, dx / speed);

            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnRow()
    {
        int spawnSpots = maxBubblesPerRow - (Frame1or2 ? 0 : 1);

        for (int i = 0; i < spawnSpots; i++)
        {
            if (Random.value > SpawnChance)
                continue;

            float spawnY = SpawnFirstPoint.position.y - (i * bubbleDiameter) - (Frame1or2 ? 0 : .5f);
            Vector3 pos = new Vector3(SpawnFirstPoint.position.x, spawnY, SpawnFirstPoint.position.z);
            Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(BubblePrefab, pos, rot, transform);
        }

        Frame1or2 = !Frame1or2;
    }
}

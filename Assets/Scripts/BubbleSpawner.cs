using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject BubblePrefab;
    [Tooltip("Optional special-case prefabs with their own spawn slices.")]
    public List<BubbleVariant> SpecialBubbleVariants = new List<BubbleVariant>();

    public Transform SpawnFirstPoint;

    [Header("Row Rules")]
    public int maxBubblesPerRow = 8;
    public float HorizontalGap = 0.1f;     // extra space between rows (world units)
    float bubbleDiameter = 1;
    public float SpawnChance = .02f;

    [Header("Fallback timing")]
    public float minWait = 0.01f; // prevents divide-by-zero / super tiny waits



    private bool Frame1or2 = true;

    [Header("Testing")]
    [SerializeField] bool garunteedSpawn = false;

    IEnumerator Start()
    {
        if (garunteedSpawn)
        {
            SpawnChance = 1f;
        }
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
            GameObject prefabToSpawn = GetBubblePrefabForSpawn();
            Instantiate(prefabToSpawn, pos, rot, transform);
        }

        Frame1or2 = !Frame1or2;
    }

    GameObject GetBubblePrefabForSpawn()
    {
        float roll = Random.value;
        float cumulative = 0f;

        for (int i = 0; i < SpecialBubbleVariants.Count; i++)
        {
            BubbleVariant variant = SpecialBubbleVariants[i];
            if (variant == null || variant.Prefab == null || variant.Chance <= 0f)
                continue;

            cumulative += Mathf.Max(0f, variant.Chance);
            if (roll < cumulative)
            {
                return variant.Prefab;
            }
        }

        return BubblePrefab;
    }

    [System.Serializable]
    public class BubbleVariant
    {
        public BubbleType type;
        public GameObject Prefab;
        [Range(0f, 1f)]
        public float Chance = 0.01f;
    }
}

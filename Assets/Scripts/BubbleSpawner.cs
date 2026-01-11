using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject BubblePrefab;
    [Tooltip("Optional special-case prefabs with their own spawn slices.")]
    public List<BubbleVariant> SpecialBubbleVariants = new List<BubbleVariant>();
    [SerializeField, Tooltip("Runtime list of bubbles that have not been popped yet.")]

    private List<Bubble> unpoppedBubbles = new List<Bubble>();
    public IReadOnlyList<Bubble> UnpoppedBubbles => unpoppedBubbles;

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
    [SerializeField] bool garunteedSpawnCorrupt = false;


    IEnumerator Start()
    {
        if (garunteedSpawn)
        {
            SpawnChance = 1f;
        }
        while (true)
        {
            SpawnRow();

            yield return new WaitForSeconds(GetDxDelaySeconds());
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
            GameObject spawnedBubble = Instantiate(prefabToSpawn, pos, rot, transform);
            RegisterBubble(spawnedBubble);

            GlobalController.Instance.BubblesSpawned += 1;
        }

        Frame1or2 = !Frame1or2;
    }

    GameObject GetBubblePrefabForSpawn()
    {
        //special corrupt bubble spawn condition
        int totalSpawns = GlobalController.Instance.BubblesSpawned;
        if    ((totalSpawns % 100 == 0 && GlobalController.Instance.TotalCorruptionTokens == 0 && totalSpawns != 0)
            || (totalSpawns % 400 == 0 && GlobalController.Instance.TotalCorruptionTokens == 1 && totalSpawns != 0)
            || (totalSpawns % 700 == 0 && totalSpawns !=0)){
            print("Spawning Corrupt Bubble! Spawn count: " + totalSpawns);
            return SpecialBubbleVariants.Find(x => x.type == BubbleType.Corrupt)?.Prefab;
        }

        //testing for garunteed corrupt
        if (totalSpawns == 0)
        {
            //print("Spawning Corrupt Bubble! Spawn count: " + totalSpawns);
            //return SpecialBubbleVariants.Find(x => x.type == BubbleType.Corrupt)?.Prefab;
        }


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

    void RegisterBubble(GameObject spawnedBubble)
    {
        if (spawnedBubble == null)
            return;

        Bubble bubbleComponent = spawnedBubble.GetComponent<Bubble>();
        if (bubbleComponent == null)
            return;

        StartCoroutine(RegisterBubbleDelayed(bubbleComponent));
    }

    IEnumerator RegisterBubbleDelayed(Bubble bubbleComponent)
    {
        float delay = GetDxDelaySeconds();
        yield return new WaitForSeconds(delay);

        if (bubbleComponent == null || bubbleComponent.Popped)
        {
            yield break;
        }

        unpoppedBubbles.Add(bubbleComponent);
        bubbleComponent.OnBubblePopped += HandleBubbleRemoved;
        bubbleComponent.OnBubbleDestroyed += HandleBubbleRemoved;
    }

    void HandleBubbleRemoved(Bubble bubble)
    {
        if (bubble == null)
            return;

        bubble.OnBubblePopped -= HandleBubbleRemoved;
        bubble.OnBubbleDestroyed -= HandleBubbleRemoved;
        unpoppedBubbles.Remove(bubble);
    }

    float GetDxDelaySeconds()
    {
        float speed = Mathf.Max(0.0001f, GlobalController.Instance.WrapperSpeed);
        float dx = bubbleDiameter + HorizontalGap;
        return Mathf.Max(minWait, dx / speed);
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

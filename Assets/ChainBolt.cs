using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBolt : MonoBehaviour
{
    const float MoveDuration = 0.075f;

    void Start()
    {
        StartCoroutine(ChainCoroutine());
    }

    void Update()
    {
        transform.position += Vector3.right * GlobalController.Instance.WrapperSpeed * Time.deltaTime;
    }

    IEnumerator ChainCoroutine()
    {
        for (int i = 0; i < GlobalController.Instance.ChainBoltMaxJumps; i++)
        {
            Bubble targetBubble = FindRandomNearestBubble();

            if (targetBubble == null)
            {
                yield return null;
                continue;
            }

            yield return MoveToBubble(targetBubble);

            if (targetBubble != null && !targetBubble.Popped)
            {
                PopData popData = new PopData(PopType.ChainBolt);
                popData.chainCount = i + 1;
                targetBubble.Pop(popData);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator MoveToBubble(Bubble targetBubble)
    {
        while (targetBubble != null && !targetBubble.Popped)
        {
            Vector3 targetPosition = targetBubble.transform.position;

            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance <= 0.03f)
            {
                transform.position = targetPosition;
                yield break;
            }

            float moveSpeed = distance / MoveDuration;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }
    }

    Bubble FindRandomNearestBubble()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GlobalController.Instance.ChainBoltRadius);
        List<Bubble> validBubbles = new List<Bubble>();
        foreach (var hitCollider in hitColliders)
        {
            Bubble bubble = hitCollider.GetComponent<Bubble>();
            if (bubble != null && !bubble.Popped)
            {
                validBubbles.Add(bubble);
            }
        }

        if (validBubbles.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validBubbles.Count);
        return validBubbles[randomIndex];
    }
}

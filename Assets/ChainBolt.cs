using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChainBolt : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ChainCoroutine());
    }

    IEnumerator ChainCoroutine()
    {
        for (int i = 0; i < GlobalController.Instance.ChainBoltMaxJumps; i++)
        {
            bool reachedBubble = false;
            Bubble targetBubble = FindRandomNearestBubble();

            if (targetBubble != null)
            {
                transform.DOMove(targetBubble.transform.position, 0.1f).onComplete += () => { reachedBubble = true; };
                Debug.Log("Chain Bolt to bubble at position: " + targetBubble.transform.position);
                targetBubble.Pop(new PopData(PopType.ChainBolt));
                while (!reachedBubble)
                {
                    yield return null;
                }
            }
        }

        Destroy(gameObject);
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

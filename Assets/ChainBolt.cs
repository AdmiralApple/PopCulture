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
        //find the nearest bubble and jump to it
        for (int i = 0; i < GlobalController.Instance.ChainBoltMaxJumps; i++)
        {
            bool reachedBubble = false;
            Bubble nearestBubble = FindNearestBubble();

            

            if (nearestBubble != null)
            {
                transform.transform.DOMove(nearestBubble.transform.position, 0.1f).onComplete += () => { reachedBubble = true; };
                //create a bolt effect from this position to the bubble position
                //you can use a LineRenderer or a ParticleSystem for this effect
                Debug.Log("Chain Bolt to bubble at position: " + nearestBubble.transform.position);
                nearestBubble.Pop(new PopData(PopType.ChainBolt));
                //wait until the bolt reaches the bubble
                while (!reachedBubble)
                {
                    yield return null;
                }

            }
        }

    }

    Bubble FindNearestBubble()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GlobalController.Instance.ChainBoltRadius);
        Bubble nearestBubble = null;
        float nearestDistance = float.MaxValue;
        foreach (var hitCollider in hitColliders)
        {
            Bubble bubble = hitCollider.GetComponent<Bubble>();
            if (bubble != null && !bubble.Popped)
            {
                float distance = Vector2.Distance(transform.position, bubble.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestBubble = bubble;
                }
            }
        }
        
        return nearestBubble;

    }
}
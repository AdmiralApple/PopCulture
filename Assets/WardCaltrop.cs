using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardCaltrop : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Bubble bubble = collision.gameObject.GetComponent<Bubble>();
            if (bubble != null && !bubble.Popped)
            {
                PopData popData = new PopData(PopType.Caltrop);
                popData.chainCount = 0; // Caltrop pops do not chain
                bubble.Pop(popData);
            }
        }
        Destroy(gameObject);
    }
}
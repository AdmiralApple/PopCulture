using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardCaltrop : MonoBehaviour
{
    bool seeking = false;
    Bubble SeekedBubble;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Caltrop collided with " + collision.gameObject.name);
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

    private void Update()
    {
        if(GlobalController.Instance.CaltropSeek == false)
        {
            return;
        }
        if (!seeking)
        {
            foreach (Bubble bubble in GlobalController.Instance.BubbleSpawner.UnpoppedBubbles)
            {
                if (!bubble.Popped)
                {
                    float distance = Vector3.Distance(transform.position, bubble.transform.position);
                    if (distance < GlobalController.Instance.CaltropSeekRange)
                    {
                        seeking = true;
                        SeekedBubble = bubble;
                        break;
                    }
                }
            }
        }
        else
        {
            if (SeekedBubble != null && !SeekedBubble.Popped)
            {
                Vector3 direction = (SeekedBubble.transform.position - transform.position).normalized;
                float speed = GlobalController.Instance.CaltropSeekSpeed;
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                seeking = false;
            }
        }


    }
}
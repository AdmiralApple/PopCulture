using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WardCaltrop : MonoBehaviour
{
    bool seeking = false;
    Bubble SeekedBubble;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionWithBubble(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollisionWithBubble(other.gameObject);
    }

    private void HandleCollisionWithBubble(GameObject target)
    {
        if (target == null)
            return;

        //Debug.Log("Caltrop collided with " + target.name);
        if (target.CompareTag("Bubble"))
        {
            Bubble bubble = target.GetComponent<Bubble>();
            if (bubble != null && !bubble.Popped)
            {
                PopData popData = new PopData(PopType.Caltrop);
                popData.chainCount = 0; // Caltrop pops do not chain
                bubble.Pop(popData);
                Destroy(gameObject);
            }
        }
        
    }

    private void Start()
    {
        //randomize rotation
        transform.Rotate(0f, 0f, Random.Range(0f, 360f));
    }

    private void Update()
    {
        if (GlobalController.Instance.CaltropSeek == false)
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

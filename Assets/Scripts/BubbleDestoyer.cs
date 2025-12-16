using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleDestoyer : MonoBehaviour
{


    //this is attached to a trigger.  destroy bubbles entering this trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            Destroy(collision.gameObject);
        }
    }
}
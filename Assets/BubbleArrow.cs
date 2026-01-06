using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleArrow : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MouseHitbox>())
        {

        }
    }
}
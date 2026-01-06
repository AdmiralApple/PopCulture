using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHitbox : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Set this to be the distance from the camera to the object
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = worldPos;

    }
}
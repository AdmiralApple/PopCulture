using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptBubble : MonoBehaviour
{
    private void OnMouseDown()
    {
        GlobalController.Instance.SetCorrupt(true);
    }
}
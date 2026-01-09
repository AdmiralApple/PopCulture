using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferenceLibrary : MonoBehaviour
{
    public static GlobalReferenceLibrary library;
    public GameObject MouseClickCircle;
    public APL_PopupText CritPopup;
    public BubbleSpawner BubbleSpawner;
    private void Awake()
    {
        if (library == null)
        {
            library = this;
        }
    }
}
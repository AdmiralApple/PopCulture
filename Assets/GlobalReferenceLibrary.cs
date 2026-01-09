using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferenceLibrary : MonoBehaviour
{
    public static GlobalReferenceLibrary library;
    public GameObject MouseClickCircle;

    private void Awake()
    {
        if (library == null)
        {
            library = this;
        }
    }
}
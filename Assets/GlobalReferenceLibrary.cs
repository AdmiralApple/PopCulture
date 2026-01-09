using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReferenceLibrary : MonoBehaviour
{
    public static GlobalReferenceLibrary library;
    public GameObject MouseClickCircle;
    public APL_PopupText CritPopup;
    public BubbleSpawner BubbleSpawner;
    public GameObject Warpper;
    public GameObject SkillTree;
    public GameObject Wrapper => Warpper;
    private void Awake()
    {
        if (library == null)
        {
            library = this;
        }
    }
}

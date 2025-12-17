using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSkillTree : MonoBehaviour
{
    public GameObject SkillTreeParent;

    public void ToggleSkillTreeVisibility()
    {
        if (SkillTreeParent != null)
        {
            bool isActive = SkillTreeParent.activeSelf;
            SkillTreeParent.SetActive(!isActive);
        }
    }
}
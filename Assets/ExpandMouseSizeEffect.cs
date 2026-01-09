using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandMouseSizeEffect : NodeData
{
    public float SizeIncrease = 1.25f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalReferenceLibrary.library.MouseClickCircle.transform.localScale *= SizeIncrease;
    }

}
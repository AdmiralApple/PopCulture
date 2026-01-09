using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseSpawnWidthEffect : NodeData
{
    public float WidthDecrease = 0.1f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalReferenceLibrary.library.BubbleSpawner.HorizontalGap -= WidthDecrease;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBossEffect : NodeData
{
    public override void Apply(SkillNodeContext context)
    {
        GlobalReferenceLibrary.library.BubbleSpawner.SpawnChance = 0;
        GlobalController.Instance.CurrentCorruptionTokens = 0;
        GlobalController.Instance.WrapperSpeed = 0;
        DestroyImmediate(GlobalReferenceLibrary.library.SkillTree);
    }
}
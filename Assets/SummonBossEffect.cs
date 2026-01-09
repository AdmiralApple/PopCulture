using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonBossEffect : NodeData
{
    public override void Apply(SkillNodeContext context)
    {
        GlobalReferenceLibrary.library.BubbleSpawner.gameObject.SetActive(false);
        GlobalController.Instance.CurrentCorruptionTokens = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenCritComboEffect : NodeData
{
    public float critOnGoldChanceDelta = .1f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.CritIncreaseOnGoldPopChance += critOnGoldChanceDelta;
    }

}
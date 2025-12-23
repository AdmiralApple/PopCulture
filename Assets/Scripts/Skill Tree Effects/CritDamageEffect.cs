using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritDamageEffect : NodeData
{
    public float critDamageDelta = .5f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.critMultiplier += critDamageDelta;
    }
}
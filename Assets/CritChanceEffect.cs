using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritChanceEffect : NodeData
{


    public float critRateDelta = .02f;

    public override void Apply(SkillNodeContext context)
    {

        GlobalController targetController = context.GlobalController;
        targetController.critChance += critRateDelta;
    }
}
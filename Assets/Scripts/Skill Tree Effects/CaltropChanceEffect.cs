using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropChanceEffect : NodeData
{
    public float caltropChanceDelta = .1f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.CaltropChance += caltropChanceDelta;
    }
}
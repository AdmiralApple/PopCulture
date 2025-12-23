using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopEffect : NodeData
{
    public float popValueDelta = 1.0f;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.PopValueModifier += popValueDelta;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowImmunityEffect : NodeData
{
    public override void Apply(SkillNodeContext context)
    {
        GlobalController.Instance.ArrowImmune = true;
    }
}
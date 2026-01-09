using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropInceptionEffect : NodeData
{
    public bool IsActive = false;

    public override void Apply(SkillNodeContext context)
    {
        GlobalController.Instance.CaltropCanSpawnCaltrop = IsActive;
    }
}
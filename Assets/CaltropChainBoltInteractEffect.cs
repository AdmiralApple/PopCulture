using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaltropChainBoltInteractEffect : NodeData
{
    public bool IsActive = false;

    public override void Apply(SkillNodeContext context)
    {
        GlobalController.Instance.CaltropChainBoltInteraction = IsActive;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBoltEffect : NodeData
{
    public float chainBoltRadiusDelta = 0.5f;
    public float chainBoltChanceDelta = 0.1f;
    public int chainBoltMaxJumpsDelta = 1;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.ChainBoltRadius += chainBoltRadiusDelta;
        targetController.ChainBoltChance += chainBoltChanceDelta;
        targetController.ChainBoltMaxJumps += chainBoltMaxJumpsDelta;
    }
}
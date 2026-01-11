using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBoltInfiniteJumpEffect : NodeData
{

    public override void Apply(SkillNodeContext context)
    {
        GlobalController.Instance.ChainBoltCanSpawnChainBolt = true;
    }
}
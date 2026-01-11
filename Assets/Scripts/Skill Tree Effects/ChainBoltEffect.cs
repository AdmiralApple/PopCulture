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

        var spawner = GlobalReferenceLibrary.library.BubbleSpawner;
        if (spawner != null)
        {
            foreach (var bubbleType in spawner.SpecialBubbleVariants)
            {
                if (bubbleType.type == BubbleType.Shield)
                {
                    bubbleType.Chance += 0.05f; // increase shield spawn chance by 5%
                }
                if (bubbleType.type == BubbleType.Archer)
                {
                    bubbleType.Chance += 0.05f; // increase archer spawn chance by 5%
                }
            }
        }

    }
}
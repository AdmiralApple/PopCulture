using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMouseSizeCircleEffect : NodeData
{
    public bool mousecircleEnabled = true;

    public override void Apply(SkillNodeContext context)
    {
        GlobalReferenceLibrary.library.MouseClickCircle.SetActive(mousecircleEnabled);

        // this is where bubbles evolve

        // increase shield and archer chances in GlobalReferenceLibrary.library.BubbleSpawner
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
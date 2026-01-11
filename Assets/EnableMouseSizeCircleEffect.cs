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
        
    }
}
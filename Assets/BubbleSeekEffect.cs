using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSeekEffect : NodeData
{
    public bool bubbleSeek = true;
    public override void Apply(SkillNodeContext context)
    {
        GlobalController targetController = context.GlobalController;
        targetController.CaltropSeek = bubbleSeek;
    }
}
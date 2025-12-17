using UnityEngine;

[CreateAssetMenu(fileName = "AutoPopEffect", menuName = "SkillTree/Auto Pop Effect")]
public class AutoPopEffect : NodeData
{


    public override void Apply(SkillNodeContext context)
    {
        context.GlobalController.AutoPop = true;
    }
}

public class AutoPopEffectPrerequisite : Prerequisite
{
    public override bool IsMet(SkillTreeNode node)
    {
        return node.Level > 0;
    }
}

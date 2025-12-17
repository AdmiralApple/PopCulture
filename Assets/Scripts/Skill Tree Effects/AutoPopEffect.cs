using UnityEngine;

[CreateAssetMenu(fileName = "AutoPopEffect", menuName = "SkillTree/Auto Pop Effect")]
public class AutoPopEffect : NodeData
{


    public override void Apply(SkillNodeContext context)
    {
        context.GlobalController.AutoPop = true;
    }

    public override void InitializeNode(SkillTreeNode node)
    {
        Prerequisites.Add(new AutoPopEffectPrerequisite());
    }
}

public class AutoPopEffectPrerequisite : Prerequisite
{
    public override bool IsMet(SkillTreeNode node)
    {

        if (node == null || node.ParentNodes == null || node.ParentNodes.Count == 0)
        {
            return false;
        }


        foreach (var parent in node.ParentNodes)
        {
            if (parent == null)
            {
                return false;
            }
            if (parent.CurrentLevel < parent.Data.MaxLevel)
            {
                return false;
            }
        }

        return true;
    }
}

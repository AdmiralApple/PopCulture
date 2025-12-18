using UnityEngine;

public class AutoPopEffect : NodeData
{


    public override void Apply(SkillNodeContext context)
    {
        context.GlobalController.AutoPop = true;
    }

}

public class AllParentsFullyUpgradedPrereq : Prerequisite
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

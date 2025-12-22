using UnityEngine;

public class AutoPopEffect : NodeData
{


    public override void Apply(SkillNodeContext context)
    {
        context.GlobalController.AutoPop = true;
    }

    public override void InitializeNode(SkillTreeNode node)
    {
        base.InitializeNode(node);
        Prerequisites.Add(new IsCorruptPrereq());

        if(UnityEngine.Application.isPlaying == false)
        {
            return;
        }
        GlobalController.Instance.OnCorrupt += () =>
        {
            foreach (var prereq in Prerequisites)
            {
                if (!prereq.IsMet(node))
                {
                    return;
                }
                print("All prereqs are met on corruption. Unlocking node " + node.name);
                foreach (var parent in node.ParentNodes)
                {
                    parent.ChildsToArrows[node].gameObject.SetActive(true);
                }
                node.gameObject.SetActive(true);
            }
        };
    }

}

public class IsCorruptPrereq : Prerequisite
{
    public override bool IsMet(SkillTreeNode node)
    {
        return GlobalController.Instance != null && GlobalController.Instance.Corrupt;
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

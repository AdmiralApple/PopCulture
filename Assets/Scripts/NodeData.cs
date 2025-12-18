using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
public abstract class NodeData : MonoBehaviour
{
    [Title("Display")]
    public string DisplayName;

    [TextArea]
    public string Description;

    [Title("Costs & Limits"), SerializeField]
    protected int BaseCost = 1;

    [MinValue(1)]
    public int MaxLevel = 1;

    public int costIncrease = 0;

    [Title("Structure")]
    [Tooltip("All prerequisite conditions must be true before this node is revealed")]
    public List<Prerequisite> Prerequisites = new();

    [Title("Effect Params")]

    public abstract void Apply(SkillNodeContext context);

    public virtual void Remove(SkillNodeContext context)
    {
        // Optional override for respec logic.
    }

    public virtual int PostCalcCost(int currentLevel)
    {
        return BaseCost + (costIncrease * currentLevel);
    }

    public virtual void InitializeNode(SkillTreeNode node)
    {
    }
}

public readonly struct SkillNodeContext
{
    public readonly GlobalController GlobalController;
    public readonly SkillTreeNode Node;

    public SkillNodeContext(SkillTreeNode node)
    {
        Node = node;
        GlobalController = GlobalController.Instance;
    }
}

public abstract class Prerequisite
{
    public abstract bool IsMet(SkillTreeNode node);
}
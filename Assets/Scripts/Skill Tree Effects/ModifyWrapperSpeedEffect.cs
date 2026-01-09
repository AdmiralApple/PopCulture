using UnityEngine;
using DG.Tweening;

public class ModifyWrapperSpeedEffect : NodeData
{
    [Tooltip("Amount added to GlobalController.WrapperSpeed when the node is purchased.")]
    public float SpeedDelta = 1f;

    public override void Apply(SkillNodeContext context)
    {
        if (context.GlobalController == null)
        {
            Debug.LogWarning("GlobalController missing. Cannot apply wrapper speed effect.");
            return;
        }



    }

    public override void Remove(SkillNodeContext context)
    {
        if (context.GlobalController == null)
        {
            return;
        }

        context.GlobalController.WrapperSpeed -= SpeedDelta;
    }
}

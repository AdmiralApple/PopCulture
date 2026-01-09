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



        context.GlobalController.DOKill(); // Kill any existing tweens on the target
        float startSpeed = context.GlobalController.WrapperSpeed;
        float endSpeed = startSpeed + SpeedDelta;
        DOTween.To(
            () => context.GlobalController.WrapperSpeed,
            x => context.GlobalController.WrapperSpeed = x,
            endSpeed,
            2f
        );
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

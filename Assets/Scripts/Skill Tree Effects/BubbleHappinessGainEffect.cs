using UnityEngine;

[CreateAssetMenu(fileName = "BubbleHappinessGainEffect", menuName = "SkillTree/Bubble Happiness Gain")]
public class BubbleHappinessGainEffect : NodeData
{
    public float SpeedDelta = .1f;

    public override void Apply(SkillNodeContext context)
    {
        if (context.GlobalController == null)
        {
            Debug.LogWarning("GlobalController missing. Cannot apply wrapper speed effect.");
            return;
        }

        context.GlobalController.WrapperSpeed += SpeedDelta;
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

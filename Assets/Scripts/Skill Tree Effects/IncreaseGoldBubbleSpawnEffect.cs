using UnityEngine;

public class IncreaseGoldBubbleSpawnEffect : NodeData
{

    [Tooltip("Negative values make the spawner faster.")]
    public float SpawnRateDelta = .005f;

    public override void Apply(SkillNodeContext context)
    {

        BubbleSpawner TargetSpawner = context.GlobalController.BubbleSpawner;

        foreach (var bubble in TargetSpawner.SpecialBubbleVariants)
        {
            if (bubble.Name == "gold")
            {
                bubble.Chance += SpawnRateDelta;
            }
        }
    }
}

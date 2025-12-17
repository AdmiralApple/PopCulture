using UnityEngine;

[CreateAssetMenu(fileName = "SpawnIntervalEffect", menuName = "SkillTree/Modify Spawn Interval")]
public class ModifySpawnIntervalEffect : NodeData
{

    [Tooltip("Negative values make the spawner faster.")]
    public float IntervalDelta = .01f;

    public override void Apply(SkillNodeContext context)
    {

        BubbleSpawner TargetSpawner = context.GlobalController?.BubbleSpawner;
        if (TargetSpawner == null)
        {
            Debug.LogWarning("Spawn interval effect has no BubbleSpawner assigned.");
            return;
        }

        TargetSpawner.SpawnChance += IntervalDelta;
    }
}

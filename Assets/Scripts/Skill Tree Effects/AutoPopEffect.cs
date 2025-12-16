using UnityEngine;

[CreateAssetMenu(fileName = "AutoPopEffect", menuName = "SkillTree/Auto Pop Effect")]
public class AutoPopEffect : NodeData
{


    public override void Apply(SkillNodeContext context)
    {
        context.GlobalController.AutoPop = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SummonBossEffect : NodeData
{

    public override void Apply(SkillNodeContext context)
    {
        


        GlobalReferenceLibrary.library.BubbleSpawner.SpawnChance = 0;
        GlobalReferenceLibrary.library.MouseClickCircle.SetActive(false);
        GlobalController.Instance.CurrentCorruptionTokens = 0;
        GlobalController.Instance.WrapperSpeed = 0;
        //DestroyImmediate(GlobalReferenceLibrary.library.SkillTree);


        //after 2 seconds, explode the skill tree

        StartCoroutine(DelayExplodeSkillTree());
    }

    private IEnumerator DelayExplodeSkillTree()
    {
        yield return new WaitForSeconds(2f);

        var skillTree = GlobalReferenceLibrary.library.SkillTree;
        if (skillTree == null) yield break;

        var exploder = ObjectExploder.Instance;
        if (exploder == null) yield break;

        GlobalController.Instance.WrapperSpeed = 0;

        while (GlobalReferenceLibrary.library.BubbleSpawner.UnpoppedBubbles.Count > 0)
        {
            GlobalReferenceLibrary.library.BubbleSpawner.UnpoppedBubbles[0].Pop(new PopData(PopType.Mouse));
        }


        exploder.ExplodeChildren(skillTree);

        GlobalReferenceLibrary.library.BossBubble.DOMoveX(0, 5f).SetEase(Ease.Linear).onComplete += () =>
        {
            GlobalReferenceLibrary.library.BossBubble.GetComponent<BossBubble>().isClickable = true;
            exploder.ExplodeChildren(GlobalReferenceLibrary.library.BubbleSpawner.gameObject);
        };
    }
}

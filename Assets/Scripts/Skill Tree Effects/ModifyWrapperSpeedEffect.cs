using System.Collections.Generic;
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

        var library = GlobalReferenceLibrary.library;
        if (library == null)
        {
            Debug.LogWarning("GlobalReferenceLibrary missing. Applying wrapper speed effect without visuals.");
            context.GlobalController.WrapperSpeed += SpeedDelta;
            return;
        }

        var wrapperObject = library.Wrapper;
        if (wrapperObject == null)
        {
            Debug.LogWarning("Wrapper reference missing. Applying wrapper speed effect without visuals.");
            context.GlobalController.WrapperSpeed += SpeedDelta;
            return;
        }







        context.GlobalController.WrapperSpeed += SpeedDelta;
        //decided to do it simpler
        return; 


        var renderers = wrapperObject.GetComponentsInChildren<Renderer>(true);
        var materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        if (materials.Count == 0)
        {
            context.GlobalController.WrapperSpeed += SpeedDelta;
            return;
        }

        const float fadeDuration = 0.5f;
        const float visibleAlpha = 0.07f;
        const string alphaProperty = "_Alpha";

        var tweenMaterials = new List<Material>();
        foreach (var material in materials)
        {
            if (!material.HasProperty(alphaProperty))
            {
                continue;
            }

            tweenMaterials.Add(material);
        }

        if (tweenMaterials.Count == 0)
        {
            context.GlobalController.WrapperSpeed += SpeedDelta;
            return;
        }

        Sequence fadeOutSequence = DOTween.Sequence();
        foreach (var material in tweenMaterials)
        {
            fadeOutSequence.Join(material.DOFloat(0f, alphaProperty, fadeDuration));
        }

        Sequence fadeInSequence = DOTween.Sequence();
        foreach (var material in tweenMaterials)
        {
            fadeInSequence.Join(material.DOFloat(visibleAlpha, alphaProperty, fadeDuration));
        }

        Sequence wrapperSequence = DOTween.Sequence();
        wrapperSequence.Append(fadeOutSequence);
        wrapperSequence.AppendCallback(() =>
        {
            context.GlobalController.WrapperSpeed += SpeedDelta;
        });
        wrapperSequence.Append(fadeInSequence);
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

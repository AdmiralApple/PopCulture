using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class APL_SlowEnable : MonoBehaviour
{
    [SerializeField] private GameObject RendererToEnable;
    [SerializeField] private float FadeInDuration = 2f;
    [SerializeField] private Ease FadeEase = Ease.OutSine;

    private readonly List<Tween> _activeTweens = new List<Tween>();

    private void Awake()
    {
        if (RendererToEnable == null)
        {
            RendererToEnable = gameObject;
        }
    }

    [Button]
    public void SlowEnable()
    {
        if (RendererToEnable == null)
        {
            Debug.LogWarning("APL_SlowEnable has no target renderer root assigned.");
            return;
        }

        KillActiveTweens();

        FadeRenderers();
        FadeCanvasRenderers();
    }

    private void FadeRenderers()
    {
        var renderers = RendererToEnable.GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (material == null || !material.HasProperty("_Color"))
                {
                    continue;
                }

                var targetAlpha = material.color.a;
                var color = material.color;
                color.a = 0f;
                material.color = color;

                var tween = material
                    .DOFade(targetAlpha, FadeInDuration)
                    .SetEase(FadeEase);

                _activeTweens.Add(tween);
            }
        }
    }

    private void FadeCanvasRenderers()
    {
        var canvasRenderers = RendererToEnable.GetComponentsInChildren<CanvasRenderer>(true);
        foreach (var canvasRenderer in canvasRenderers)
        {
            var targetAlpha = canvasRenderer.GetAlpha();
            canvasRenderer.SetAlpha(0f);

            var tween = DOTween
                .To(canvasRenderer.GetAlpha, canvasRenderer.SetAlpha, targetAlpha, FadeInDuration)
                .SetEase(FadeEase);

            _activeTweens.Add(tween);
        }
    }

    private void KillActiveTweens()
    {
        foreach (var tween in _activeTweens)
        {
            tween?.Kill();
        }

        _activeTweens.Clear();
    }
}

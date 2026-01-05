using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        FadeGraphicComponents();
        FadeCanvasRenderers();
    }

    [Button]
    public void SetAllInvisible()
    {
        if (RendererToEnable == null)
        {
            Debug.LogWarning("APL_SlowEnable has no target renderer root assigned.");
            return;
        }

        KillActiveTweens();
        SetRendererAlpha(0f);
        SetGraphicAlpha(0f);
        SetCanvasRendererAlpha(0f);
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
                if (Mathf.Approximately(targetAlpha, 0f))
                {
                    targetAlpha = 1f;
                }
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

    private void FadeGraphicComponents()
    {
        var graphics = RendererToEnable.GetComponentsInChildren<Graphic>(true);
        foreach (var graphic in graphics)
        {
            var targetAlpha = graphic.color.a;
            if (Mathf.Approximately(targetAlpha, 0f))
            {
                targetAlpha = 1f;
            }

            var color = graphic.color;
            color.a = 0f;
            graphic.color = color;

            var tween = graphic
                .DOFade(targetAlpha, FadeInDuration)
                .SetEase(FadeEase);

            _activeTweens.Add(tween);
        }
    }

    private void SetRendererAlpha(float alpha)
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

                var color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }

    private void FadeCanvasRenderers()
    {
        var canvasRenderers = RendererToEnable.GetComponentsInChildren<CanvasRenderer>(true);
        foreach (var canvasRenderer in canvasRenderers)
        {
            if (canvasRenderer.GetComponent<Graphic>() != null)
            {
                continue;
            }

            var targetAlpha = canvasRenderer.GetAlpha();
            if (Mathf.Approximately(targetAlpha, 0f))
            {
                targetAlpha = 1f;
            }
            canvasRenderer.SetAlpha(0f);

            var tween = DOTween
                .To(canvasRenderer.GetAlpha, canvasRenderer.SetAlpha, targetAlpha, FadeInDuration)
                .SetEase(FadeEase);

            _activeTweens.Add(tween);
        }
    }

    private void SetGraphicAlpha(float alpha)
    {
        var graphics = RendererToEnable.GetComponentsInChildren<Graphic>(true);
        foreach (var graphic in graphics)
        {
            var color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    private void SetCanvasRendererAlpha(float alpha)
    {
        var canvasRenderers = RendererToEnable.GetComponentsInChildren<CanvasRenderer>(true);
        foreach (var canvasRenderer in canvasRenderers)
        {
            if (canvasRenderer.GetComponent<Graphic>() != null)
            {
                continue;
            }

            canvasRenderer.SetAlpha(alpha);
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

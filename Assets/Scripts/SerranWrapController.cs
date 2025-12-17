using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SerranWrapController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Tooltip("World-space width covered by one UV span. Leave at 0 to auto-measure from the renderer bounds.")]
    private float manualWorldWidth = 0f;

    private static readonly int SpeedProperty = Shader.PropertyToID("_Speed");
    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        propertyBlock = new MaterialPropertyBlock();
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying || GlobalController.Instance == null || targetRenderer == null)
        {
            return;
        }

        float width = GetWorldWidth();
        if (width <= 0.0001f)
        {
            width = 1f;
        }

        // Shader expects UV-units-per-second; convert world speed by dividing by the width that spans 0→1 UV.
        float shaderSpeed = GlobalController.Instance.WrapperSpeed / width;

        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(SpeedProperty, shaderSpeed);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            propertyBlock = new MaterialPropertyBlock();
            targetRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat(SpeedProperty, 0);
            targetRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    private float GetWorldWidth()
    {
        if (manualWorldWidth > 0f)
        {
            return manualWorldWidth;
        }

        return targetRenderer.bounds.size.x;
    }
}

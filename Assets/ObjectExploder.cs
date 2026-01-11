using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static UnityEngine.GraphicsBuffer;

public class ObjectExploder : MonoBehaviour
{
    [SerializeField] private float explosionForce = 5f;
    public static ObjectExploder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ExplodeAll()
    {
        var transforms = FindObjectsOfType<Transform>(true);
        foreach (var target in transforms)
        {
            TryExplodeTransform(target);
        }
    }

    public void ExplodeChildren(GameObject parent)
    {

        var children = parent.GetComponentsInChildren<Transform>(true);
        foreach (var child in children)
        {
            //if (child == parent.transform) continue;
            TryExplodeTransform(child);
        }
    }

    private void TryExplodeTransform(Transform target)
    {
        if (target == null) return;
        if (target.GetComponent<ExplosionImmune>() != null) return;

        var rb = target.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = target.gameObject.AddComponent<Rigidbody2D>();
        }


        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 direction = (Vector2)target.position;
        if (direction == Vector2.zero)
        {
            direction = Random.insideUnitCircle;
        }

        direction.Normalize();
        rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
    }

    [Button("Explode All Objects")]
    private void ExplodeAllButton()
    {
        ExplodeChildren(GlobalReferenceLibrary.library.SkillTree);
    }
}

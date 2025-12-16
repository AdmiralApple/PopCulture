using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeNode : MonoBehaviour
{
    private static readonly Dictionary<NodeData, SkillTreeNode> NodeLookup = new();

    [Title("Data")]
    public NodeData Data;

    [ShowInInspector, ReadOnly]
    public int CurrentLevel { get; private set; }

    [Title("Visuals")]
    public LineRenderer ConnectionLine;
    public List<GameObject> ChildNodes = new();

    public bool IsUnlocked => CurrentLevel > 0;

    public TextMeshPro LevelText;

    private void OnEnable()
    {
        RegisterNode();
        UpdateVisuals();
    }

    private void OnDisable()
    {
        UnregisterNode();
    }

    private void RegisterNode()
    {
        if (Data == null)
        {
            Debug.LogWarning($"{name} has no NodeData assigned.");
            return;
        }

        if (NodeLookup.TryGetValue(Data, out var existing) && existing != this)
        {
            Debug.LogWarning($"NodeData {Data.name} already registered to {existing.name}. Overwriting with {name}.");
        }

        NodeLookup[Data] = this;
    }

    private void UnregisterNode()
    {
        if (Data == null)
        {
            return;
        }

        if (NodeLookup.TryGetValue(Data, out var existing) && existing == this)
        {
            NodeLookup.Remove(Data);
        }
    }

    public bool CanPurchase()
    {
        if (Data == null)
        {
            return false;
        }

        if (CurrentLevel >= Data.MaxLevel)
        {
            return false;
        }

        var controller = GlobalController.Instance;
        if (controller == null || controller.TotalPops < Data.Cost)
        {
            return false;
        }

        return MeetsPrerequisites();
    }

    public bool TryPurchase()
    {
        if (!CanPurchase())
        {
            return false;
        }

        var controller = GlobalController.Instance;
        controller.TotalPops -= Data.Cost;

        CurrentLevel++;

        if (Data != null)
        {
            Data.Apply(new SkillNodeContext(this));
        }
        else
        {
            Debug.LogWarning($"{name} has no effect assigned.");
        }

        return true;
    }

    private bool MeetsPrerequisites()
    {
        if (Data == null || Data.Prerequisites == null || Data.Prerequisites.Count == 0)
        {
            return true;
        }

        foreach (var prerequisite in Data.Prerequisites)
        {
            if (prerequisite == null)
            {
                continue;
            }

            if (!NodeLookup.TryGetValue(prerequisite, out var node) || !node.IsUnlocked)
            {
                return false;
            }
        }

        return true;
    }

    [Button(ButtonSizes.Large)]
    private void DebugPurchase()
    {
        TryPurchase();
    }

    [Button]
    public void RebuildArrowGraph()
    {
        if (ConnectionLine == null)
        {
            Debug.LogWarning($"{name} is missing a connection line prefab.");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child != ConnectionLine.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        foreach (var child in ChildNodes)
        {
            if (child == null)
            {
                continue;
            }

            var line = Instantiate(ConnectionLine, transform.position, Quaternion.identity, transform);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, child.transform.position);
        }
    }


    private void OnMouseDown()
    {
        if(TryPurchase())
        {
            Debug.Log($"Purchased node: {name}");
            UpdateVisuals();
        }
        else
        {
            Debug.Log($"Failed to purchase node: {name}");
        }
    }

    private void UpdateVisuals()
    {
        LevelText.text = CurrentLevel.ToString() + "/" + Data.MaxLevel;
    }
}

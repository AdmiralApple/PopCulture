using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeNode : MonoBehaviour
{
    private static readonly Dictionary<NodeData, SkillTreeNode> NodeLookup = new();

    [Title("Data")]
    public NodeData Data;
    public bool RootNode = false;
    [ShowInInspector, ReadOnly]
    public int CurrentLevel { get; private set; }

    [Title("Visuals")]
    public LineRenderer ConnectionLine;
    public List<SkillTreeNode> ChildNodes = new();
    public List<SkillTreeNode> ParentNodes = new();
    private Dictionary<SkillTreeNode, LineRenderer> ChildsToArrows = new();  //maps child nodes to their connection lines

    public SkillNodeTooltip Tooltip;

    public bool IsUnlocked => CurrentLevel > 0;

    public TextMeshPro LevelText;

    [Title("Debug")]
    [SerializeField] private bool hideNodesOnRebuildInspector = false;

    public static bool HideNodesOnRebuild { get; private set; }

    private void OnEnable()
    {
        RegisterNode();
        UpdateVisuals();
    }

    private void OnDisable()
    {
        UnregisterNode();
    }

    private void OnValidate()
    {
        HideNodesOnRebuild = hideNodesOnRebuildInspector;
    }

    private void Start()
    {
        RebuildArrowGraph();
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
        if (controller == null || controller.CurrentPops < Data.PostCalcCost(CurrentLevel))
        {
            return false;
        }

        return true;
    }

    public bool TryPurchase()
    {
        if (!CanPurchase())
        {
            return false;
        }

        var controller = GlobalController.Instance;
        controller.RemovePops(Data.PostCalcCost(CurrentLevel));

        CurrentLevel++;

        print("Node " + name + " purchased. New level: " + CurrentLevel);
        print("ChildCount: " + ChildNodes.Count);
        foreach (var child in ChildNodes)
        {
            foreach (var prereq in child.Data.Prerequisites)
            {
                if (!prereq.IsMet(child))
                {
                    Debug.Log($"Child node {child.name} prerequisites not met.");
                    continue;
                }
            }
            ChildsToArrows[child].gameObject.SetActive(true);
            child.gameObject.SetActive(true);
        }

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


    [Button(ButtonSizes.Large)]
    public void RebuildArrowGraph()
    {

        if (RootNode)
        {
            ClearParents();
        }

        ChildsToArrows = new Dictionary<SkillTreeNode, LineRenderer>();

        if (ConnectionLine == null)
        {
            Debug.LogWarning($"{name} is missing a connection line prefab.");
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.GetComponent<LineRenderer>())
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

            child.ParentNodes.Add(this);

            LineRenderer line = Instantiate(ConnectionLine, transform.position, Quaternion.identity, transform);
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, child.transform.position - transform.position);

            ChildsToArrows[child] = line;

            if (HideNodesOnRebuild)
            {
                line.gameObject.SetActive(false);
                child.gameObject.SetActive(false);
            }
            else {
                line.gameObject.SetActive(true);
                child.gameObject.SetActive(true);
            }

            child.RebuildArrowGraph();  
        }
    }

    public void ClearParents()
    {
        ParentNodes.Clear();
        foreach (var child in ChildNodes)
        {
            child.ParentNodes.Clear();
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
        Tooltip.SetData(this);
    }


    private void OnMouseEnter()
    {
        Tooltip.gameObject.SetActive(true);
        Tooltip.SetData(this);
    }

    private void OnMouseExit()
    {
        Tooltip.gameObject.SetActive(false);
    }
}

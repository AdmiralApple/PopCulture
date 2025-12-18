using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix;
public class SkillTreeNode : MonoBehaviour
{
    private static readonly Dictionary<NodeData, SkillTreeNode> NodeLookup = new();

    [Title("Data")]
    public NodeData Data;
    public GameObject CanBuyIndicator;
    public bool RootNode = false;
    [ShowInInspector, ReadOnly]
    public int CurrentLevel { get; private set; }

    [Title("Visuals")]
    public LineRenderer ConnectionLine;
    public List<SkillTreeNode> ChildNodes = new();
    public List<SkillTreeNode> ParentNodes = new();
    [SerializeField]private Dictionary<SkillTreeNode, LineRenderer> ChildsToArrows = new();  //maps child nodes to their connection lines

    public SkillNodeTooltip Tooltip;

    public bool IsUnlocked => CurrentLevel > 0;

    public TextMeshPro LevelText;

    [Title("Debug")]
    [SerializeField] public bool HideNodesOnRebuild = true;

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
        //RebuildArrowGraph();
    }


    private void Start()
    {
        if (RootNode)
        {
            RebuildArrowGraph();
        }
    }

    private void Update()
    {
        if (CanBuyIndicator != null)
        {
            CanBuyIndicator.SetActive(CanPurchase());
        }
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
            bool prereqFailed = false;
            print(child.Data.name);
            print(child.Data.Prerequisites.Count + " prerequisites to check.");
            foreach (Prerequisite prereq in child.Data.Prerequisites)
            {
                if (!prereq.IsMet(child))
                {
                    Debug.Log($"Child node {child.name} prerequisites not met.");
                    prereqFailed = true;
                }
            }
            if (prereqFailed) continue;

            print(child.name + " prerequisites met. Unlocking node.");
            foreach (var parent in child.ParentNodes)
            {
                print($"Parent: {parent.name}, Level: {parent.CurrentLevel}/{parent.Data.MaxLevel}");
                parent.ChildsToArrows[child].gameObject.SetActive(true);
            }
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

        Data.InitializeNode(this);

        if (RootNode)
        {
            ClearParents();
        }

        var oldArrows = ChildsToArrows;
        ChildsToArrows = new Dictionary<SkillTreeNode, LineRenderer>();
        if (oldArrows != null)
        {
            foreach (var line in oldArrows.Values)
            {
                if (line == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(line.gameObject);
                }
                else
                {
                    DestroyImmediate(line.gameObject);
                }
            }
        }

        if (ConnectionLine == null)
        {
            Debug.LogWarning($"{name} is missing a connection line prefab.");
            return;
        }

        foreach (Transform child in transform)
        {
            if (!child.GetComponent<LineRenderer>())
            {
                continue;
            }

            if (Application.isPlaying)
            {
                Destroy(child.gameObject);
            }
            else
            {
                DestroyImmediate(child.gameObject);
            }
        }
        print(gameObject.name + " Child count: " + ChildNodes.Count);

        foreach (var child in ChildNodes)
        {
            if (child == null)
            {
                continue;
            }

            print($"Connecting {name} to child {child.name}");
            child.ParentNodes.Add(this);

            LineRenderer line = Instantiate(ConnectionLine, transform.position, Quaternion.identity, transform);
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, child.transform.position - transform.position);

            ChildsToArrows[child] = line;

            line.gameObject.SetActive(true);
            child.gameObject.SetActive(true);

            child.HideNodesOnRebuild = HideNodesOnRebuild;
            child.RebuildArrowGraph();  
        }

        if (HideNodesOnRebuild)
        {
            HideChildren();
        }
    }

    public void ClearParents()
    {
        print($"Clearing parents of node {name}");
        ParentNodes.Clear();
        foreach (var child in ChildNodes)
        {
            child.ClearParents();
        }
    }

    public void HideChildren()
    {
        foreach (var child in ChildNodes)
        {
            if (child == null)
            {
                continue;
            }
            if (ChildsToArrows.TryGetValue(child, out var line))
            {
                line.gameObject.SetActive(false);
            }
            child.gameObject.SetActive(false);
            child.HideChildren();
        }
    }

    [Button(ButtonSizes.Large)]
    public void PrintChildToArrowsSize()
    {
        Debug.Log($"Node {name} has {ChildsToArrows.Count} child arrows.");
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

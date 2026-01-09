using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillNodeTooltip : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI DescriptionText;

    public void SetData(SkillTreeNode skillTreeNode)
    {
        if (skillTreeNode == null || skillTreeNode.Data == null)
        {
            TitleText.text = "No Data";
            CostText.text = "";
            DescriptionText.text = "This skill node has no data assigned.";
            return;
        }
        NodeData data = skillTreeNode.Data;
        bool hideData = data.IsCorruptNode && !skillTreeNode.IsUnlocked;
        if (hideData)
        {
            TitleText.text = "???";
            CostText.text = "Cost: ???";
            DescriptionText.text = "???";
            return;
        }

        TitleText.text = data.DisplayName;
        CostText.text = BuildCostText(data, skillTreeNode);
        DescriptionText.text = data.Description;
    }

    private string BuildCostText(NodeData data, SkillTreeNode node)
    {
        List<string> costParts = new();
        int popCost = data.PostCalcCost(node.CurrentLevel);
        if (popCost > 0)
        {
            costParts.Add($"{popCost} Pops");
        }

        if (data.IsCorruptNode)
        {
            costParts.Add("1 Corruption Token");
        }

        if (costParts.Count == 0)
        {
            return "Cost: Free";
        }

        return $"Cost: {string.Join(" + ", costParts)}";
    }
}

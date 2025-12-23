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
        TitleText.text = data.DisplayName;
        CostText.text = $"Cost: {data.PostCalcCost(skillTreeNode.CurrentLevel)}";
        DescriptionText.text = data.Description;
    }
}
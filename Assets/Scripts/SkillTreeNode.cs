using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeNode : MonoBehaviour
{
    public LineRenderer ConnectionLine;
    public List<GameObject> ChildNodes;

    public int Cost = 1;
    public int MaxLevel = 5;


    [Button]
    public void RebuildArrowGraph()
    {
        //destroy children
        foreach (Transform child in transform)
        {
            if (child != ConnectionLine.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        foreach (var child in ChildNodes)
        {
            var line = Instantiate(ConnectionLine, transform.position, Quaternion.identity, transform);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, child.transform.position);
        }
    }
}
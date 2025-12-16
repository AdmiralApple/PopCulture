using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;

    void Update()
    {

        counter.text = "Total Pops: " + GlobalController.Instance.TotalPops.ToString();
    }

}
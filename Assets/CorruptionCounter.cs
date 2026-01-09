using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorruptionCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;

    void Update()
    {

        counter.text = "CORRUPTION " + GlobalController.Instance.CurrentCorruptionTokens.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedlyMadePopCountChecker : MonoBehaviour
{
    public APL_SlowEnable aplSlowEnable;
    bool revieled = false;
    public int PopCouhntCheck = 10;

    // Update is called once per frame
    void Update()
    {
        if (!revieled && GlobalController.Instance.TotalPops >= PopCouhntCheck)
        {
            aplSlowEnable.SlowEnable();
            revieled = true;
        }
    }
}
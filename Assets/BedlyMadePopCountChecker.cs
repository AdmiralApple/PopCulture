using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedlyMadePopCountChecker : MonoBehaviour
{
    public APL_SlowEnable aplSlowEnable;
    bool revieled = false;
    public int PopCouhntCheck = 10;
    public bool CheckCorruption = false;

    // Update is called once per frame
    void Update()
    {
        if (CheckCorruption)
        {
            if (GlobalController.Instance.Corrupt)
            {
                if (!revieled)
                {
                    aplSlowEnable.SlowEnable();
                    revieled = true;
                }
                return;
            }
            return;
        }
        if (!revieled && GlobalController.Instance.TotalPops >= PopCouhntCheck)
        {
            aplSlowEnable.SlowEnable();
            revieled = true;
        }
    }
}
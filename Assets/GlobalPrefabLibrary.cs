using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPrefabLibrary : MonoBehaviour
{

    public static GlobalPrefabLibrary Instance;
    public GameObject ChainBoltPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
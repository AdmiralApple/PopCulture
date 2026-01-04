using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPrefabLibrary : MonoBehaviour
{

    public static GlobalPrefabLibrary Instance;
    public GameObject ChainBoltPrefab;
    public GameObject CaltropPrefab;

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
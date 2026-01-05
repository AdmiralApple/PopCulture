using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class APL_SlowEnable : MonoBehaviour
{
    public GameObject RendererToEnable;
    public float FadeInDuration = 2f;

    private void Start()
    {
        if(RendererToEnable != null)
        {
            RendererToEnable = this.gameObject;
        }
    }

    [Button]
    public void SlowEnable()
    {

    }
}
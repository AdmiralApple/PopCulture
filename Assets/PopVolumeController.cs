using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopVolumeController : MonoBehaviour
{
    public void SetPopVolume(float volume)
    {
        Bubble.PopVolume = volume;
    }
}
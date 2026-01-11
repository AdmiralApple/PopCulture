using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossBubble : MonoBehaviour
{
    public bool isClickable = false;
    public TextMeshProUGUI HappinessCounter;
    public PopCounter HappinessPopCounter;
    private void OnMouseDown()
    {

        if (!isClickable) return;

    }

    private IEnumerator TriggerEndCutscene()
    {
        yield return new WaitForSeconds(2f);
        HappinessPopCounter.enabled = false;

        for (int i = 0; i < 100; i++)
        {
            //set text to "Happiness: " plus an amount of random numbers equal to Fibbinoccie(i)

            yield return new WaitForSeconds(0.05f);
        }
    }
}
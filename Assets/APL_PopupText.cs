using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class APL_PopupText : MonoBehaviour
{
    public float RelativeHeight = 0;
    public float ExpansionSize = 2;
    public Vector3 OriginalSize;

    [Button(ButtonSizes.Large)]
    public void BeginPopup() {
        OriginalSize = transform.localScale;

        Sequence popupSequence = DOTween.Sequence();
        popupSequence.Append(transform.DOMoveY(transform.position.y + RelativeHeight, 0.5f).SetEase(Ease.OutCubic));
        popupSequence.Join(transform.DOScale(OriginalSize * ExpansionSize, 0.25f).SetEase(Ease.OutCubic));
        popupSequence.Append(transform.DOScale(OriginalSize, 0.25f).SetEase(Ease.InCubic));
        popupSequence.OnComplete(() => Destroy(gameObject));

    } 
    
}
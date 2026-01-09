using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartGame : MonoBehaviour
{
    public Transform Wrapper;
    public Transform WrapperBox;
    public Transform StartButtonWorldCanvas;
    public APL_SlowEnable VolumeSlider;

    public float openingSpeed = 2f;
    public Ease openingEase = Ease.InSine;

    public void QueueStart()
    {



        WrapperBox.DOLocalMoveX(-10.97012f, openingSpeed).SetEase(openingEase);
        StartButtonWorldCanvas.DOLocalMoveX(-18.689479f, openingSpeed).SetEase(openingEase);
        Wrapper.DOLocalMoveX(1, openingSpeed).SetEase(openingEase);
        Wrapper.DOScaleX(20f, openingSpeed).SetEase(openingEase).OnComplete(() =>
        {
            GlobalController.Instance.BubbleSpawner.gameObject.SetActive(true);
            VolumeSlider.gameObject.SetActive(true);
            VolumeSlider.SlowEnable();
        });

    }

}
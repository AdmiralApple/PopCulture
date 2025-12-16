using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 5f;
    public float Value = 1f;

    public GameObject UnpoppedSprite;
    public GameObject PoppedSprite;
    public bool Popped = false;

    //moves right from speed

    void Update()
    {
        //move right, ignoring rotation
        transform.position += Vector3.right * GlobalController.Instance.WrapperSpeed * Time.deltaTime;


    }

    //on click, pop the bubble
    private void OnMouseDown()
    {
        Pop();
    }

    void Pop()
    {
        if(Popped) return;
        UnpoppedSprite.SetActive(false);
        PoppedSprite.SetActive(true);

        GlobalController.Instance.TotalPops += Value;

        Popped = true;
    }
}
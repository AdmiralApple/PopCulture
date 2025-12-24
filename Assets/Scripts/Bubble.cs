using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float BaseValue = 1f;

    public GameObject UnpoppedSprite;
    public GameObject PoppedSprite;
    public bool Popped = false;

    public BubbleType Type = BubbleType.Normal;

    //moves right from speed

    void Update()
    {
        //move right, ignoring rotation
        transform.position += Vector3.right * GlobalController.Instance.WrapperSpeed * Time.deltaTime;


    }

    //on click, pop the bubble
    private void OnMouseDown()
    {
        PopData popData = new PopData(PopType.Mouse);
        Pop(popData);
    }

    private void OnMouseEnter()
    {
        if (GlobalController.Instance.AutoPop)
        {
            PopData popData = new PopData(PopType.Mouse);
            Pop(popData);
        }
    }

    public void Pop(PopData pData)
    {
        if(Popped) return;
        if(pData.chainCount > GlobalController.Instance.ChainBoltMaxJumps) return;
        UnpoppedSprite.SetActive(false);
        PoppedSprite.SetActive(true);


        switch(Type)
        {
            case BubbleType.Golden:
                float critChanceIncreaseChance = Random.Range(0f, 1f);
                if (critChanceIncreaseChance < GlobalController.Instance.CritIncreaseOnGoldPopChance)
                {
                    GlobalController.Instance.critChance += 0.01f;
                    Debug.Log("Crit Chance Increased! New Crit Chance: " + GlobalController.Instance.critChance);
                }
                break;
            case BubbleType.Corrupt:
                GlobalController.Instance.SetCorrupt(true);
                break;
            default:
                break;
        }

        float popValue = BaseValue + GlobalController.Instance.PopValueModifier;

        //crit
        float critRoll = Random.Range(0f, 1f);
        if (GlobalController.Instance.critChance > critRoll)
        {
            //critical pop
            Debug.Log("Critical Pop!");

            popValue *= GlobalController.Instance.critMultiplier;
        }
        GlobalController.Instance.AddPops(popValue);

        Popped = true;




        //chain bolt
        float chainRoll = Random.Range(0f, 1f);
        if (GlobalController.Instance.ChainBoltChance > chainRoll)
        {
            
            Instantiate(GlobalPrefabLibrary.Instance.ChainBoltPrefab, transform.position, Quaternion.identity);
        }
    }
}

public class PopData
{

    public PopData(PopType type)
    {
        Type = type;
        chainCount = 0;
    }


    public PopType Type;
    public int chainCount;

}

public enum PopType
{
    Mouse,
    Critical,
    ChainBolt,
    Caltrop
}
public enum BubbleType
{
    Normal,
    Corrupt,
    Golden,
    Shield
}
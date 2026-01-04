using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float BaseValue = 1f;

    public GameObject UnpoppedSprite;
    public GameObject PoppedSprite;
    public bool Popped = false;

    public event Action<Bubble> OnBubblePopped;
    public event Action<Bubble> OnBubbleDestroyed;

    public BubbleType Type = BubbleType.Normal;

    //moves right from speed

    //move right, ignoring rotation
    void Start()
    {
        // Ensure the bubble has a Rigidbody2D for physics-based movement
        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f; // No gravity for bubbles
            rb.freezeRotation = true; // Prevent rotation
        }
    }

    void Update()
    {
        // Set velocity directly using Rigidbody2D for physics-based movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * GlobalController.Instance.WrapperSpeed;
        }
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
                float critChanceIncreaseChance = UnityEngine.Random.Range(0f, 1f);
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
        float critRoll = UnityEngine.Random.Range(0f, 1f);
        if (GlobalController.Instance.critChance > critRoll)
        {
            //critical pop
            Debug.Log("Critical Pop!");

            popValue *= GlobalController.Instance.critMultiplier;
        }
        GlobalController.Instance.AddPops(popValue);

        Popped = true;
        OnBubblePopped?.Invoke(this);

        //chain bolt

        if (pData.Type != PopType.ChainBolt || GlobalController.Instance.ChainBoltCanSpawnChainBolt)
        {
            float chainRoll = UnityEngine.Random.Range(0f, 1f);
            if (GlobalController.Instance.ChainBoltChance > chainRoll)
            {

                Instantiate(GlobalPrefabLibrary.Instance.ChainBoltPrefab, transform.position, Quaternion.identity);
            }
        }

        //caltrop
        if (pData.Type != PopType.Caltrop)
        {
            float caltropRoll = UnityEngine.Random.Range(0f, 1f);
            if (GlobalController.Instance.CaltropChance > caltropRoll)
            {
                GameObject caltrop = Instantiate(GlobalPrefabLibrary.Instance.CaltropPrefab, transform.position, Quaternion.identity);
            }
        }

    }

    private void OnDestroy()
    {
        OnBubbleDestroyed?.Invoke(this);
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

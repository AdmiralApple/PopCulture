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
        Pop(0);
    }

    private void OnMouseEnter()
    {
        if (GlobalController.Instance.AutoPop)
        {
            Pop(0);
        }
    }

    void Pop(int chainCount)
    {
        if(Popped) return;
        if(chainCount > GlobalController.Instance.ChainBoltMaxJumps) return;
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
            //find nearby bubbles within radius
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GlobalController.Instance.ChainBoltRadius);
            foreach (var hitCollider in hitColliders)
            {
                Bubble bubble = hitCollider.GetComponent<Bubble>();
                if (bubble != null && !bubble.Popped)
                {
                    bubble.Pop(chainCount + 1);
                    break; //only pop one bubble
                }
            }
        }
    }
}

public enum BubbleType
{
    Normal,
    Corrupt,
    Golden,
    Shield
}
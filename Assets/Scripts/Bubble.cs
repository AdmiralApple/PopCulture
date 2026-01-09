using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Bubble : MonoBehaviour
{
    public float BaseValue = 1f;

    public GameObject UnpoppedSprite;
    public GameObject PoppedSprite;
    public bool Popped = false;

    public event Action<Bubble> OnBubblePopped;
    public event Action<Bubble> OnBubbleDestroyed;

    public BubbleType Type = BubbleType.Normal;

    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    public List<AudioClip> PopSounds;

    public static float PopVolume = 1.0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        if (circleCollider != null)
        {
            circleCollider.isTrigger = false;
        }
    }

    void Update()
    {
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
        Popped = true;
        OnBubblePopped?.Invoke(this);
        PlayRandomPopSound();

        switch (Type)
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

            SpawnCritPopup();

            popValue *= GlobalController.Instance.critMultiplier;
        }
        GlobalController.Instance.AddPops(popValue);

        

        //chain bolt

        if (pData.Type != PopType.ChainBolt || (pData.Type == PopType.ChainBolt && GlobalController.Instance.ChainBoltCanSpawnChainBolt) || (GlobalController.Instance.CaltropChainBoltInteraction && pData.Type == PopType.Caltrop))
        {
            float chainRoll = UnityEngine.Random.Range(0f, 1f);
            if (GlobalController.Instance.ChainBoltChance > chainRoll)
            {

                Instantiate(GlobalPrefabLibrary.Instance.ChainBoltPrefab, transform.position, Quaternion.identity);
            }
        }

        //caltrop
        if (pData.Type != PopType.Caltrop || (pData.Type == PopType.Caltrop && GlobalController.Instance.CaltropCanSpawnCaltrop) || (GlobalController.Instance.CaltropChainBoltInteraction && pData.Type == PopType.ChainBolt))
        {
            float caltropRoll = UnityEngine.Random.Range(0f, 1f);
            if (GlobalController.Instance.CaltropChance > caltropRoll)
            {
                GameObject caltrop = Instantiate(GlobalPrefabLibrary.Instance.CaltropPrefab, transform.position, Quaternion.identity);
            }
        }

    }

    void SpawnCritPopup()
    {
        GameObject popupObj = Instantiate(GlobalReferenceLibrary.library.CritPopup.gameObject, transform.position, Quaternion.identity);
        APL_PopupText popupText = popupObj.GetComponent<APL_PopupText>();
        popupText.BeginPopup();
    }

    private void OnDestroy()
    {
        OnBubbleDestroyed?.Invoke(this);
    }

    void PlayRandomPopSound()
    {
        if (PopSounds.Count == 0) return;
        int index = UnityEngine.Random.Range(0, PopSounds.Count);
        AudioClip clip = PopSounds[index];

        // Create a temporary audio source to add pitch and volume variation
        GameObject tempAudio = new GameObject("TempPopAudio");
        tempAudio.transform.position = transform.position;
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.0f); // Slight pitch variation

        if(Type == BubbleType.Corrupt)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.2f, 0.4f); // Lower pitch for corrupt bubbles
        }

        audioSource.volume = UnityEngine.Random.Range(0.4f, 1.0f); // Slight volume variation
        audioSource.volume *= PopVolume; // Apply global pop volume
        audioSource.Play();
        Destroy(tempAudio, clip.length / audioSource.pitch);
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

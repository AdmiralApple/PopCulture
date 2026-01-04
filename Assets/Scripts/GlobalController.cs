using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public static GlobalController Instance;
    public BubbleSpawner BubbleSpawner;
    public float CurrentPops { get; private set; } = 0;
    public float TotalPops { get; private set; } = 0;

    public float WrapperSpeed = 3;

    [Title("Global Variables")]
    public bool Corrupt = false;
    public bool AutoPop = false;  //if true, the player pops bubbles on mouse over

    public float PopValueModifier = 0;

    public float critChance = 0.00f; //5% crit chance
    public float critMultiplier = 2.0f; //2x damage on crit

    public float CritIncreaseOnGoldPopChance = 0.00f; //increase crit chance by 1% on gold pop

    public float ChainBoltRadius = 1; //on pop, chance to chain bolt to nearby bubbles
    public float ChainBoltChance = 0; //chance to chain bolt on pop
    public int ChainBoltMaxJumps = 1; //max jumps for chain bolt
    public bool ChainBoltCanSpawnChainBolt = false; //if true, chain bolts can spawn more chain bolts



    public float CaltropChance = 0; //chance to spawn caltrops on pop
    public bool CaltropSeek  = false; //if true, caltrops will seek nearby enemies
    public float CaltropSeekRange = 1; //range at which caltrops will seek enemies
    public float CaltropSeekSpeed = 5; //speed at which caltrops will seek enemies

    public int BubblesSpawned = 0; //total number of popped bubbles


    //make an event that gets called when you become corrupt
    public event System.Action OnCorrupt;

    public void SetCorrupt(bool isCorrupt)
    {
        if (Corrupt != isCorrupt)
        {
            Corrupt = isCorrupt;
            if (Corrupt && OnCorrupt != null)
            {
                OnCorrupt.Invoke();
            }
        }
    }




    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddPops(float amount)
    {
        CurrentPops += amount;
        TotalPops += amount;
    }

    public void RemovePops(float amount)
    {
        CurrentPops = Mathf.Max(0, CurrentPops - amount);
    }


}
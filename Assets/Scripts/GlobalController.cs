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

    public float BasePopValue = 1;


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
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
    public bool AutoPop = false;  //if true, the player pops bubbles on mouse over

    public float BasePopValue = 1;

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
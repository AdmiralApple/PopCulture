using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APL_HotkeyButton : MonoBehaviour
{
    public List<KeyCode> Hotkeys;

    public void Update()
    {
        foreach (KeyCode key in Hotkeys)
        {
            if (Input.GetKeyDown(key))
            {
                this.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }
        }
    }
}
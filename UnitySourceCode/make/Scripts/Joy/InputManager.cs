using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private static InputManager inst;
    public static InputManager Inst
    {
        get
        {
            if (inst == null)
            {
                GameObject go = new GameObject("InputManager");
                inst = go.AddComponent<InputManager>();
            }
            return inst;
        }
    }

    public Vector2 Axis { get; set; }
    public bool IsJoystickMove { get; set; }
}




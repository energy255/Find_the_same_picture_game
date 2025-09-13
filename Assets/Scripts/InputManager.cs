using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [HideInInspector]
    public bool IsClick;

    void Update()
    {
        IsClick = Input.GetMouseButtonDown(0);
    }
}

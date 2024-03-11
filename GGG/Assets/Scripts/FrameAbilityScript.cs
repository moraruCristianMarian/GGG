using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FrameAbilityScript : MonoBehaviour
{
    public UnityEvent MyAbility;

    void Awake()
    {
        if (MyAbility == null)
            MyAbility = new UnityEvent();
    }

    public void UseAbility()
    {
        MyAbility.Invoke();
    }
}

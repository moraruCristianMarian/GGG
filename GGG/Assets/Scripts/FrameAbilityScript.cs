using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FrameAbilityScript : MonoBehaviour
{
    public UnityEvent MyAbility;
    public UnityEvent MyActiveAbility;
    public GameObject MyActiveAbilityIcon;

    void Awake()
    {
        if (MyAbility == null)
            MyAbility = new UnityEvent();
        if (MyActiveAbility == null)
            MyActiveAbility = new UnityEvent();
    }

    public void UseAbility()
    {
        MyAbility.Invoke();
    }
    public void UseActiveAbility()
    {
        MyActiveAbility.Invoke();
    }

    void OnDestroy()
    {
        if (MyActiveAbilityIcon)
            Destroy(MyActiveAbilityIcon);
    }
}

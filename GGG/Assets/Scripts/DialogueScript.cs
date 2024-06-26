using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class DialogueScript
{
    public string Name;
    public Sprite SpeakerSprite;
    [TextAreaAttribute]
    public string Speech;
    public bool Mirrored;
    public UnityEvent AppearEvent;

    public void DoAppearEvent()
    {
        if (AppearEvent != null)
            AppearEvent.Invoke();
    }
}

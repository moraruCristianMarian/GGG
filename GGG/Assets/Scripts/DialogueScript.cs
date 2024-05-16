using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueScript
{
    public string Name;
    public Sprite SpeakerSprite;
    [TextAreaAttribute]
    public string Speech;
    public bool Mirrored;
}

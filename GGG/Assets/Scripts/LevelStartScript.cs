using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    public DialogueUserScript PreLevelDialogue;
    void Start()
    {
        PreLevelDialogue.TriggerDialogue();
    }
}

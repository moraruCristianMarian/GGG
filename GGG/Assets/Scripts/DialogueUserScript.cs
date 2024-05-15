using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueUserScript : MonoBehaviour
{
    public DialogueScript[] MyDialogues;    

    public void TriggerDialogue()
    {
        DialogueSingletonScript.Get().StartDialogue(MyDialogues);
    }
}

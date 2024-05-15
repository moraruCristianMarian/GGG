using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSingletonScript : MonoBehaviour
{
    public GameObject DialogueDisplay;
    public Image SpeakerImage;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI SpeechText;

    private Queue<DialogueScript> _dialogues;
    private static DialogueSingletonScript _singleton;

    void Awake()
    {
        _dialogues = new Queue<DialogueScript>();
        _singleton = this;
    }
    public static DialogueSingletonScript Get()
    {
        return _singleton;
    }

    public void StartDialogue(DialogueScript[] newDialogues)
    {
        _dialogues.Clear();
        foreach (DialogueScript nd in newDialogues)
            _dialogues.Enqueue(nd);

        DialogueDisplay.SetActive(true);
        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueScript dia = _dialogues.Dequeue();

        SpeakerImage.sprite = dia.SpeakerSprite;
        SpeakerNameText.text = dia.Name;
        SpeechText.text = dia.Speech;
    }

    void EndDialogue()
    {
        DialogueDisplay.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            DisplayNextDialogue();
    }
}

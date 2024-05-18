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
    public GameObject EnterToContinuePrompt;

    private Queue<DialogueScript> _dialogues;
    private static DialogueSingletonScript _singleton;
    private bool _finishedTypingSpeech = true;
    private string _fullSpeech = "";

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
        if (!_finishedTypingSpeech)
        {
            StopAllCoroutines();
            _finishedTypingSpeech = true;
            SpeechText.text = _fullSpeech;
            return;
        }

        if (_dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueScript dia = _dialogues.Dequeue();

        SpeakerImage.sprite = dia.SpeakerSprite;
        SpeakerNameText.text = dia.Name;
        StartCoroutine(TypeSpeech(dia.Speech));

        EnterToContinuePrompt.SetActive(false);

        dia.DoAppearEvent();
    }

    IEnumerator TypeSpeech(string speech)
    {
        _finishedTypingSpeech = false;
        _fullSpeech = speech;

        SpeechText.text = "";
        foreach (char letter in speech.ToCharArray())
        {
            SpeechText.text += letter;
            yield return null;
        }

        _finishedTypingSpeech = true;
        EnterToContinuePrompt.SetActive(true);
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

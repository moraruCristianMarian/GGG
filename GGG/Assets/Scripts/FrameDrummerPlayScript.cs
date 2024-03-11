using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDrummerPlayScript : MonoBehaviour
{
    public GameObject GoblinDrummerObject;
    public float PlayDelay = 0.5f;

    private Animator _goblinDrummerAnimator;
    private FrameCommandScript _sendCommandScript;
    private bool _canPlay = true;
    private float _drumCooldown = 0.1f;

    private void Awake()
    {
        _goblinDrummerAnimator = GoblinDrummerObject.GetComponent<Animator>();
        _sendCommandScript = gameObject.GetComponent<FrameCommandScript>();
    }

    public void PlayDrumsWait()
    {
        if (_canPlay)
        {
            _canPlay = false;
            StartCoroutine(AbilityCooldown());
            StartCoroutine(PlayDrums());
        }
    }

    private IEnumerator PlayDrums()
    {
        yield return new WaitForSeconds(PlayDelay);

        _goblinDrummerAnimator.SetTrigger("Playing");

        _sendCommandScript.SendCommand();
    }

    private IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(_drumCooldown);

        _canPlay = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class AbilityButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject MyFramePiece;
    public Material MaterialHighlighted;
    public FrameAbilityScript MyUnitAbility;
    public TextMeshProUGUI ChargesLeftText;
    public Button AbilityButton;
    public Image Image;
    public Image ImageCooldown;
    public float AbilityMaxCooldown = 1.0f;
    public int ChargesLeft = -1;
    private bool _usedAbility = false;
    private Material _materialNotHighlighted;
    private Material _materialNotHighlightedGoblin;
    private bool _highlighted = false;

    public void UseAbility()
    {
        if ((!_usedAbility) && (ChargesLeft > 0))
        {
            ImageCooldown.fillAmount = 1;
            AbilityButton.interactable = false;
            _usedAbility = true;

            MyUnitAbility.UseActiveAbility();

            ChargesLeft -= 1;
            ChargesLeftText.text = ChargesLeft.ToString();
            if (ChargesLeft == 0)
                ImageCooldown.fillAmount = 1;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (MyFramePiece)
        {
            _highlighted = true;

            _materialNotHighlighted = MyFramePiece.GetComponent<SpriteRenderer>().material;
            MyFramePiece.GetComponent<SpriteRenderer>().material = MaterialHighlighted;

            if (MyFramePiece.GetComponent<FramePieceScript>().MyGoblin)
            {
                _materialNotHighlightedGoblin = MyFramePiece.GetComponent<FramePieceScript>().MyGoblin.GetComponent<SpriteRenderer>().material;
                MyFramePiece.GetComponent<FramePieceScript>().MyGoblin.GetComponent<SpriteRenderer>().material = MaterialHighlighted;
            }
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (MyFramePiece)
        {
            _highlighted = false;

            MyFramePiece.GetComponent<SpriteRenderer>().material = _materialNotHighlighted;
            if (MyFramePiece.GetComponent<FramePieceScript>().MyGoblin)
                MyFramePiece.GetComponent<FramePieceScript>().MyGoblin.GetComponent<SpriteRenderer>().material = _materialNotHighlightedGoblin;
        }
    }

    void OnDestroy()
    {
        if ((MyFramePiece) && (_highlighted))
        {
            MyFramePiece.GetComponent<SpriteRenderer>().material = _materialNotHighlighted;
            if (MyFramePiece.GetComponent<FramePieceScript>().MyGoblin)
                MyFramePiece.GetComponent<FramePieceScript>().MyGoblin.GetComponent<SpriteRenderer>().material = _materialNotHighlightedGoblin;
        }
    }

    void Start()
    {
        if (ChargesLeft != -1)
            ChargesLeftText.text = ChargesLeft.ToString();
        if (ChargesLeft == 0)
            ImageCooldown.fillAmount = 1;
    }

    void Update()
    {
        if ((_usedAbility) && (ChargesLeft != 0))
        {
            ImageCooldown.fillAmount -= Time.deltaTime / AbilityMaxCooldown;

            if (ImageCooldown.fillAmount <= 0)
            {
                ImageCooldown.fillAmount = 0;
                AbilityButton.interactable = true;
                _usedAbility = false;
            }
        }
    }
}

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

    public void UseAbility()
    {
        if (!_usedAbility)
        {
            ImageCooldown.fillAmount = 1;
            AbilityButton.interactable = false;
            _usedAbility = true;

            MyUnitAbility.UseActiveAbility();

            if (ChargesLeft > 0)
            {
                ChargesLeft -= 1;
                ChargesLeftText.text = ChargesLeft.ToString();
                if (ChargesLeft == 0)
                    ImageCooldown.fillAmount = 1;
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (MyFramePiece)
        {
            _materialNotHighlighted = MyFramePiece.GetComponent<SpriteRenderer>().material;
            MyFramePiece.GetComponent<SpriteRenderer>().material = MaterialHighlighted;
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (MyFramePiece)
            MyFramePiece.GetComponent<SpriteRenderer>().material = _materialNotHighlighted;
    }

    void Start()
    {
        if (ChargesLeft != -1)
            ChargesLeftText.text = ChargesLeft.ToString();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AbilityButtonScript : MonoBehaviour
{
    public Button AbilityButton;
    public Image Image;
    public Image ImageCooldown;
    public float AbilityMaxCooldown = 1.0f;
    public FrameAbilityScript MyUnitAbility;
    private bool _usedAbility = false;

    public void UseAbility()
    {
        if (!_usedAbility)
        {
            ImageCooldown.fillAmount = 1;
            AbilityButton.interactable = false;
            _usedAbility = true;

            MyUnitAbility.UseActiveAbility();
        }
    }

    void Update()
    {
        if (_usedAbility)
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
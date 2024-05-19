using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAbilitiesScript : MonoBehaviour
{
    public GameObject AbilityTooltip;
    public GameObject AbilityIcon;
    public GameObject KingAbility;
    public GameObject OnCommand;

    public void ShowAbilityTooltip()
    {
        AbilityTooltip.SetActive(true);
    }
    public void ShowAbilityIcon()
    {
        AbilityTooltip.SetActive(false);
        AbilityIcon.SetActive(true);
    }
    public void ShowKingAbility()
    {
        AbilityIcon.SetActive(false);
        KingAbility.SetActive(true);
    }
    public void ShowOnCommand()
    {
        KingAbility.SetActive(false);
        OnCommand.SetActive(true);
    }
}

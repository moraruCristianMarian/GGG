using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CreateAbilityButtonScript : MonoBehaviour
{
    public Sprite AbilityIcon;
    // public UnityEvent MyAbility;

    public GameObject AbilityIconPrefab;

    private GameObject _myAbilityIcon;

    public void CreateMyAbilityButton()
    {
        GameObject AbilityBar = GameObject.FindWithTag("AbilityBar");
        if (AbilityBar)
        {
            _myAbilityIcon = Instantiate(AbilityIconPrefab);
            _myAbilityIcon.transform.SetParent(AbilityBar.transform, false);

            float myBarOffset = (AbilityBar.transform.childCount - 1) * AbilityIcon.rect.width * 3;
            _myAbilityIcon.transform.position += new Vector3(myBarOffset, 0, 0);
            
            AbilityButtonScript abs = _myAbilityIcon.GetComponentInChildren<AbilityButtonScript>();
            if (abs)
            {
                abs.MyUnitAbility = gameObject.GetComponent<FrameAbilityScript>();
                abs.Image.sprite = AbilityIcon;
                abs.ImageCooldown.sprite = AbilityIcon;
            }
        }
    }

    // public void UseAbility()
    // {
    //     MyAbility.Invoke();
    // }
}

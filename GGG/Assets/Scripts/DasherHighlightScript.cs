using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DasherHighlightScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool _held = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _held = true;

            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            _held = false;
    }

    void Update()
    {
        if (_held)
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            
            Vector3 dir = mouseWorldPos - transform.position;
            dir.z = 0;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            float snapAngle = Mathf.Round(angle / 15) * 15;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, snapAngle));
        }
    }

    public void SetRocketRotation()
    {
        transform.parent.GetComponent<GoblinDasherAbilityScript>().DashQuaternion = transform.rotation;


        //  Create indicator above the Goblin Dasher's ability icon for the rotation of the dash direction
        GameObject myActiveAbilityIcon = transform.parent.GetComponent<CreateAbilityButtonScript>().GetMyActiveAbilityIcon();

        GameObject iconRotationIndicatorPrefab = transform.parent.GetComponent<GoblinDasherAbilityScript>().IconRotationIndicatorPrefab;
        GameObject iconRotationIndicator = Instantiate(iconRotationIndicatorPrefab);
        iconRotationIndicator.transform.SetParent(myActiveAbilityIcon.transform, false);
        iconRotationIndicator.transform.rotation = transform.rotation;
        iconRotationIndicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(48, 48);


        //  Destroy this highlight
        Destroy(gameObject);
    }
}

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
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void SetRocketRotation()
    {
        transform.parent.GetComponent<GoblinDasherAbilityScript>().DashQuaternion = transform.rotation;
        Destroy(gameObject);
    }
}

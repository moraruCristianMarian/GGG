using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WheelHighlightScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject WheelPrefab;
    private bool _held = false;
    private Vector2 _heldObjectOffset;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            transform.SetParent(null);
            _held = true;

            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _heldObjectOffset = mouseWorldPos - transform.position;
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
            
            transform.position = (Vector2)mouseWorldPos - _heldObjectOffset;
        }
    }

    void AttachAWheel(GameObject parentObject, bool rightWheel)
    {
        float wheelX = rightWheel ? 0.3f : -0.3f;
        float wheelY = -0.52f;

        GameObject wheel = Instantiate(WheelPrefab);
        wheel.transform.SetParent(parentObject.transform);

        wheel.transform.localPosition = new Vector2(wheelX, wheelY);

        WheelJoint2D wheelJoint = parentObject.AddComponent<WheelJoint2D>();
        wheelJoint.connectedBody = wheel.GetComponent<Rigidbody2D>();
        wheelJoint.enableCollision = false;
        wheelJoint.anchor = new Vector2(wheelX, wheelY);
        wheelJoint.suspension = new JointSuspension2D
        {
            angle = 0f,
            frequency = 300f,
            dampingRatio = 1f
        };
    }

    public void AttachWheelsToFramePiece()
    {
        //  Find the frame piece this highlight is attached to
        FramePieceScript[] framePieceScripts = FindObjectsOfType<FramePieceScript>();
        FramePieceScript nearestFps = null;

        float minDist = 2.0f;
        foreach (FramePieceScript fps in framePieceScripts)
        {
            var dist = (fps.transform.position - transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                nearestFps = fps;
            }
        }
        if (!nearestFps)
        {
            Debug.Log("no neighbours ;[");
            Destroy(gameObject);
            return;
        }

        //  Spawn and attach two wheels to the found frame piece
        AttachAWheel(nearestFps.gameObject, false);
        AttachAWheel(nearestFps.gameObject, true);

        Destroy(gameObject);
    }
}

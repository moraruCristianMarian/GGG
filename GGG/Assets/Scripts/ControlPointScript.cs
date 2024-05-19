using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlPointScript : MonoBehaviour
{
    public float CaptureRate = 1.0f;
    public SpriteRenderer FillImage;
    public UnityEvent CaptureEvent;
    private bool _captured = false;

    public void DoCaptureEvent()
    {
        if (CaptureEvent != null)
            CaptureEvent.Invoke();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!_captured)
        {
            if (!other.gameObject.HasCustomTag("FramePiece"))
                return;

            float captureAmount = FillImage.size.y + Time.deltaTime * CaptureRate;
            FillImage.size = new Vector2(1, captureAmount);

            if (FillImage.size.y >= 1)
            {
                FillImage.size = new Vector2(1, 1);
                _captured = true;

                DoCaptureEvent();
            }
        }
    }
}

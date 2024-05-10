using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMechanicScript : MonoBehaviour
{
    public GameObject WheelHighlight;

    void OnDestroy()
    {
        if (WheelHighlight)
            Destroy(WheelHighlight);
    }
}

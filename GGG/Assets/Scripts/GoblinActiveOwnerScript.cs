using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinActiveOwnerScript : MonoBehaviour
{
    private FrameAbilityScript _baseFrameAbilityScript;
    void Start()
    {
        _baseFrameAbilityScript = transform.parent.gameObject.GetComponent<FrameAbilityScript>();
    }

    void OnDestroy()
    {
        if ((_baseFrameAbilityScript) && (_baseFrameAbilityScript.MyActiveAbilityIcon))
            Destroy(_baseFrameAbilityScript.MyActiveAbilityIcon);
    }
}

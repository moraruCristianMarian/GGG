using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolScript : MonoBehaviour
{
    public bool AllowEntry = false;
    
    public void MaxAlpha()
    {
        var maxAlphaCol = gameObject.GetComponent<SpriteRenderer>().color;
        maxAlphaCol.a = 1f;
        gameObject.GetComponent<SpriteRenderer>().color = maxAlphaCol;
    }
}

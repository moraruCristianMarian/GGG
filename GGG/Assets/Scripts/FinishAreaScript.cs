using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishAreaScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.HasCustomTag("CenterPiece"))
        {
            FindObjectOfType<WinConScript>().YouWin();
        }
    }
}

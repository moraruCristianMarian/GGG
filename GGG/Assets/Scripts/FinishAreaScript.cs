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
            Debug.Log("WOW!!");

            // GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");
            // foreach (GameObject goblin in goblins)
            //     goblin.transform.SetParent(null);
        }
    }
}

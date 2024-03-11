using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCommandScript : MonoBehaviour
{
    public void SendCommand()
    {
        Debug.Log("commanding");
        List<GameObject> neighbours = gameObject.GetComponent<FramePieceScript>().GetNeighbours();
        foreach (GameObject neighbour in neighbours)
        {
            Debug.Log(neighbour.name);
            FrameAbilityScript abilityScript = neighbour.GetComponent<FrameAbilityScript>();
            if (abilityScript)
            {
                abilityScript.UseAbility();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCommandScript : MonoBehaviour
{
    public void SendCommand()
    {
        List<GameObject> neighbours = gameObject.GetComponent<FramePieceScript>().GetNeighbours();
        foreach (GameObject neighbour in neighbours)
        {
            FrameAbilityScript abilityScript = neighbour.GetComponent<FrameAbilityScript>();
            if (abilityScript)
            {
                abilityScript.UseAbility();
            }
        }
    }
}

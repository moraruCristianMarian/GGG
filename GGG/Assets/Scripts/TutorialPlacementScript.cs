using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlacementScript : MonoBehaviour
{
    public GameObject ConnectedConfig;
    public GameObject UnconnectedConfig;

    public void Connected()
    {
        ConnectedConfig.SetActive(true);
    }

    public void Unconnected()
    {
        ConnectedConfig.SetActive(false);
        UnconnectedConfig.SetActive(true);
    }
}

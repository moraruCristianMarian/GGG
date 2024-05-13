using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Yes, this is spelled with two l's
public class KingGobllinScript : MonoBehaviour
{
    void OnDestroy()
    {
        GameOverScript gos = FindObjectOfType<GameOverScript>();
        if (gos)
            gos.GameOverStart();
    }
}

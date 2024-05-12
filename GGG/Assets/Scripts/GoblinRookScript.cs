using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinRookScript : MonoBehaviour
{
    private GameObject _myGoblin;
    private GameObject _kingGoblin;
    private bool _kingGoblinFound = false;

    void Start()
    {
        _myGoblin = gameObject.GetComponent<FramePieceScript>().MyGoblin;
    }

    GameObject FindKingGoblin()
    {
        GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");
        foreach (GameObject goblin in goblins)
            if (goblin.HasCustomTag("KingGoblin"))
                return goblin;
        return null;
    }
    public void SwapWithKing()
    {
        if (!_kingGoblinFound)
            _kingGoblin = FindKingGoblin();

        if ((_kingGoblin) && (_myGoblin))
        {
            Vector2 localPosRook = _myGoblin.transform.localPosition;
            Vector2 localPosKing = _kingGoblin.transform.localPosition;


            Transform myGoblinFrame = _myGoblin.transform.parent;

            _myGoblin.transform.SetParent(_kingGoblin.transform.parent);
            _kingGoblin.transform.SetParent(myGoblinFrame);

            
            _myGoblin.transform.localPosition = localPosRook;
            _kingGoblin.transform.localPosition = localPosKing;

            //  Since the king's piece changed, the entire hierarchy needs to be rebuilt
            PlacementGridScript.ParentPieces(myGoblinFrame.gameObject);
        }
    }
}

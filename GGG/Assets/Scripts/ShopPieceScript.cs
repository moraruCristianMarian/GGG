using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopPieceScript : MonoBehaviour, IPointerDownHandler
{
    public GameObject SpawnedPiecePrefab;
    public int ActiveAbilityCharges = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject spawnedPiece = Instantiate(SpawnedPiecePrefab);
        spawnedPiece.GetComponent<FramePieceScript>().gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        //  If the spawned piece has an active ability, set its charge count
        CreateAbilityButtonScript cabs = spawnedPiece.GetComponent<CreateAbilityButtonScript>();
        if (cabs)
            cabs.ChargesLeft = ActiveAbilityCharges;

        PlacementGridScript placementGridScript = FindObjectOfType<PlacementGridScript>();
        if (placementGridScript)
        {
            placementGridScript.SetHeldObject(spawnedPiece, true);
        }
    }
}

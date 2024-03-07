using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopPieceScript : MonoBehaviour, IPointerDownHandler
{
    public Sprite PieceSprite;
    public GameObject SpawnedPiecePrefab;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject spawnedPiece = Instantiate(SpawnedPiecePrefab);
        spawnedPiece.GetComponent<SpriteRenderer>().sprite = PieceSprite;

        PlacementGridScript placementGridScript = FindObjectOfType<PlacementGridScript>();
        if (placementGridScript)
        {
            placementGridScript.SetHeldObject(spawnedPiece, true);
        }
    }
}

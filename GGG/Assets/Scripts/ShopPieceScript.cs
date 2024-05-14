using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopPieceScript : MonoBehaviour, IPointerDownHandler
{
    public Image PieceImage;
    public GameObject SpawnedPiecePrefab;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI QuantityText;
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

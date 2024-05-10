using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour
{
    public GameObject ShopPiecePrefab;
    public List<Sprite> ShopPieceSprites;

    public List<GameObject> GamePiecePrefabs;

    public List<int> ShopStockIndices;
    public List<int> ShopStockPrices;
    public List<int> ShopStockQuantity;
    public List<int> ShopStockAbilityCharges;

    private void Start()
    {
        RectTransform shopContentPanel = GetComponent<RectTransform>();
        shopContentPanel.sizeDelta = new Vector2(shopContentPanel.sizeDelta.x, ShopStockIndices.Count * 100);

        for (int i = 0; i < ShopStockIndices.Count; i++)
        {
            GameObject shopPiece = Instantiate(ShopPiecePrefab);

            shopPiece.transform.SetParent(gameObject.transform, false);
            shopPiece.transform.position = new Vector2(0, -(i+1) * 75);

            shopPiece.GetComponent<UnityEngine.UI.Image>().sprite = ShopPieceSprites[ShopStockIndices[i]];
            shopPiece.GetComponent<UnityEngine.UI.Image>().sprite = ShopPieceSprites[ShopStockIndices[i]];

            shopPiece.GetComponent<ShopPieceScript>().SpawnedPiecePrefab = GamePiecePrefabs[ShopStockIndices[i]];
            shopPiece.GetComponent<ShopPieceScript>().ActiveAbilityCharges = ShopStockAbilityCharges[i];
        }
    }
}

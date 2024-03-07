using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour
{
    public GameObject PiecePrefab;
    public List<Sprite> SpritesFrame;
    public List<Sprite> SpritesGoblin;

    public List<int> ShopStockIndices;
    public List<int> ShopStockPrices;
    public List<int> ShopStockQuantity;

    private void Start()
    {
        RectTransform shopContentPanel = GetComponent<RectTransform>();
        shopContentPanel.sizeDelta = new Vector2(shopContentPanel.sizeDelta.x, ShopStockIndices.Count * 100);

        for (int i = 0; i < ShopStockIndices.Count; i++)
        {
            GameObject shopPiece = Instantiate(PiecePrefab);

            shopPiece.transform.SetParent(gameObject.transform, false);
            shopPiece.transform.position = new Vector2(0, -(i+1) * 75);

            shopPiece.GetComponent<UnityEngine.UI.Image>().sprite = SpritesFrame[ShopStockIndices[i]];
            shopPiece.GetComponent<ShopPieceScript>().PieceSprite = SpritesFrame[ShopStockIndices[i]];
        }
    }
}

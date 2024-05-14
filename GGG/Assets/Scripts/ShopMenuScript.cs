using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour
{
    public GameObject ShopPiecePrefab;

    //  This varies from level to level, depending on which pieces are available
    [System.Serializable]
    public struct LevelPiece
    {
        public int PieceID;
        public int Price;
        public int Quantity;
        public int AbilityCharges;
    }
    public List<LevelPiece> LevelPieceData;

    //  This is always the same, having general information on the pieces (name, description, etc.)
    [System.Serializable]
    public struct GlobalPiece
    {
        public GameObject PiecePrefab;
        public Sprite ShopSprite;
        public string PieceName;
        public string PieceDesc;
    }
    public List<GlobalPiece> GlobalPieceData;


    private void Start()
    {
        RectTransform shopContentPanel = GetComponent<RectTransform>();
        shopContentPanel.sizeDelta = new Vector2(shopContentPanel.sizeDelta.x, LevelPieceData.Count * 100);

        for (int i = 0; i < LevelPieceData.Count; i++)
        {
            GlobalPiece myPiece = GlobalPieceData[LevelPieceData[i].PieceID];

            GameObject shopPiece = Instantiate(ShopPiecePrefab);

            shopPiece.transform.SetParent(gameObject.transform, false);
            shopPiece.transform.position = new Vector2(0, -(i+1) * 75);

            //  Level-specific data
            shopPiece.GetComponent<ShopPieceScript>().ActiveAbilityCharges = LevelPieceData[i].AbilityCharges;
            shopPiece.GetComponent<ShopPieceScript>().PriceText.text = string.Format("${0}", LevelPieceData[i].Price);
            shopPiece.GetComponent<ShopPieceScript>().QuantityText.text = string.Format("x{0}", LevelPieceData[i].Quantity);

            //  General data
            shopPiece.GetComponent<ShopPieceScript>().SpawnedPiecePrefab = myPiece.PiecePrefab;
            shopPiece.GetComponent<ShopPieceScript>().PieceImage.sprite = myPiece.ShopSprite;
            shopPiece.GetComponent<TooltipUserScript>().Title = myPiece.PieceName;
            shopPiece.GetComponent<TooltipUserScript>().Content = myPiece.PieceDesc;
        }
    }
}

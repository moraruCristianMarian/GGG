using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMenuScript : MonoBehaviour
{
    public int Money;
    public TextMeshProUGUI MoneyText;
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
    private ShopPieceScript[] _shopPieceScriptsByIndex;


    private void Start()
    {
        UpdateMoneyText();
        
        RectTransform shopContentPanel = GetComponent<RectTransform>();
        shopContentPanel.sizeDelta = new Vector2(shopContentPanel.sizeDelta.x, LevelPieceData.Count * 100);

        _shopPieceScriptsByIndex = new ShopPieceScript[LevelPieceData.Count];

        for (int i = 0; i < LevelPieceData.Count; i++)
        {
            GlobalPiece myPiece = GlobalPieceData[LevelPieceData[i].PieceID];

            GameObject shopPiece = Instantiate(ShopPiecePrefab);
            _shopPieceScriptsByIndex[i] = shopPiece.GetComponent<ShopPieceScript>();

            shopPiece.transform.SetParent(gameObject.transform, false);
            shopPiece.transform.position = new Vector2(0, -(i+1) * 75);

            //  Level-specific data
            _shopPieceScriptsByIndex[i].ActiveAbilityCharges = LevelPieceData[i].AbilityCharges;
            _shopPieceScriptsByIndex[i].PriceText.text = string.Format("${0}", LevelPieceData[i].Price);
            _shopPieceScriptsByIndex[i].QuantityText.text = string.Format("x{0}", LevelPieceData[i].Quantity);
            _shopPieceScriptsByIndex[i].ShopIndex = i;

            //  General data
            _shopPieceScriptsByIndex[i].TheShopMenuScript = this;
            _shopPieceScriptsByIndex[i].SpawnedPiecePrefab = myPiece.PiecePrefab;
            _shopPieceScriptsByIndex[i].PieceImage.sprite = myPiece.ShopSprite;
            shopPiece.GetComponent<TooltipUserScript>().Title = myPiece.PieceName;
            shopPiece.GetComponent<TooltipUserScript>().Content = myPiece.PieceDesc;
        }
    }

    public bool BuyPiece(int shopIndex)
    {
        if (shopIndex == -1)
            return false;

        if (Money < LevelPieceData[shopIndex].Price)
            return false;

        if (LevelPieceData[shopIndex].Quantity <= 0)
            return false;

        //  Success

        LevelPiece dataWithNewQuantity = LevelPieceData[shopIndex];
        dataWithNewQuantity.Quantity -= 1;
        LevelPieceData[shopIndex] = dataWithNewQuantity;

        _shopPieceScriptsByIndex[shopIndex].SetQuantityText(dataWithNewQuantity.Quantity);

        Money -= LevelPieceData[shopIndex].Price;
        UpdateMoneyText();

        return true;
    }
    public bool SellPiece(int shopIndex)
    {
        if (shopIndex == -1)
            return false;

        //  Success

        LevelPiece dataWithNewQuantity = LevelPieceData[shopIndex];
        dataWithNewQuantity.Quantity += 1;
        LevelPieceData[shopIndex] = dataWithNewQuantity;
        
        _shopPieceScriptsByIndex[shopIndex].SetQuantityText(dataWithNewQuantity.Quantity);

        Money += LevelPieceData[shopIndex].Price;
        UpdateMoneyText();

        return true;
    }

    private void UpdateMoneyText()
    {
        MoneyText.text = string.Format("${0}", Money);
    }
}

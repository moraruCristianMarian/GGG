using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour
{
    public GameObject PiecePrefab;
    public List<int> Prices;
    public List<Sprite> Sprites;

    private void Start()
    {
        RectTransform shopContentPanel = GetComponent<RectTransform>();
        shopContentPanel.sizeDelta = new Vector2(shopContentPanel.sizeDelta.x, Sprites.Count * 100);

        for (int i = 0; i < Sprites.Count; i++)
        {
            GameObject shopPiece = Instantiate(PiecePrefab);

            shopPiece.transform.SetParent(gameObject.transform, false);
            shopPiece.transform.position = new Vector2(0, -(i+1) * 75);

            shopPiece.GetComponent<UnityEngine.UI.Image>().sprite = Sprites[i];
        }
    }
}

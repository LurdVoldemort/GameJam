using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cardUISide : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardNameText;

    private cards cardData;

    public void SetCard(cards card)
    {
        cardData = card;

        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardNameText;
        card.cardUIPrefab = this;
    }

    public void used()
    {
        GetComponent<Image>().color = Color.gray;
    }

    public void ready()
    {
        GetComponent<Image>().color = Color.white;
    }
}

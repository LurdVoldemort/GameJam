using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class cardUI : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardNameText;

    private cards cardData;
    private CardSelector selectionUI;
    private bool selected;

    public void SetCard(cards card, CardSelector ui)
    {
        cardData = card;
        selectionUI = ui;

        cardImage.sprite = card.cardImage;
        cardNameText.text = card.cardNameText;
    }

    public void OnClick()
    {
        if (selectionUI.ToggleSelect(cardData))
        {
            selected = !selected;
            GetComponent<Image>().color = selected ? Color.green : Color.white;
        }
        
    }
}


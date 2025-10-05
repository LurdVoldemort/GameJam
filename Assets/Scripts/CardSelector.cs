using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CardSelector : MonoBehaviour
{
    public Transform cardParent;      // A UI panel (with GridLayoutGroup)
    public GameObject cardUIPrefab;   // A prefab for displaying one card
    public Button playButton;
    public Sprite cardSprite;
    public TextMeshProUGUI popupText;

    private List<cards> selectedCards = new List<cards>();

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject cardObj = new GameObject("speedBoost");
            speedBoost card = cardObj.AddComponent<speedBoost>();

            card.cardNameText = "Speed Boost";
            card.cardDescriptionText = "Makes you fast";
            card.length = Random.Range(1f, 3f);
            card.usageRate = Random.Range(5f, 10f);
            card.cardImage = cardSprite;

            // Assign some placeholder sprite if you want
            // card.cardImage = someSpriteReference;
            persistentInventory.addCard(card);
        }



        // Create a visual card for each in the persistent list
        foreach (cards cardington in persistentInventory.cardList)
        {
            GameObject cardObject = Instantiate(cardUIPrefab, cardParent);
            cardUI ui = cardObject.GetComponent<cardUI>();
            ui.SetCard(cardington, this);
        }

        playButton.onClick.AddListener(OnPlayClicked);
    }

    public bool ToggleSelect(cards card)
    {
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            return true;
        }
        else
        {
            if (selectedCards.Count < 3)
            {
                selectedCards.Add(card);
                return true;
            }
            else {
                ShowPopup("You may only have 3 cards selected!");
                return false;
            
            }
        }
    }

    private void ShowPopup(string message)
    {
        popupText.text = message;
        popupText.gameObject.SetActive(true);
        StopAllCoroutines(); // Cancel any previous fadeout
        StartCoroutine(HidePopupAfterDelay(5f)); // show for 5 seconds
    }

    private IEnumerator HidePopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupText.gameObject.SetActive(false);
    }

    private void OnPlayClicked()
    {
        if (selectedCards.Count == 3)
        {
            CardManager.Instance.selectedCards = new List<cards>(selectedCards);
            SceneManager.LoadScene("NextSceneName");
        }
        else
        {
            ShowPopup("You must select three cards to continue!");
        }
    }
}


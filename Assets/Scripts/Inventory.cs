using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{

    public PickupItem[] inventory;
    public persistentInventory persistentInventory;
    public Player_Movement player;

    //UI for sidescreen
    public Transform cardParent;
    public GameObject cardUIPrefab;
    public Sprite cardSprite;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; i++)
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
            player.addCard(card);
        }

        GameObject cardObj2 = new GameObject("speedBoost");
        scatterShot card2 = cardObj2.AddComponent<scatterShot>();

        card2.cardNameText = "Scatter Shot";
        card2.cardDescriptionText = "Makes you fast";
        card2.length = Random.Range(1f, 10f);
        card2.usageRate = Random.Range(0.5f, 1.5f);
        card2.cardImage = cardSprite;

        // Assign some placeholder sprite if you want
        // card.cardImage = someSpriteReference;
        persistentInventory.addCard(card2);
        player.addCard(card2);

        foreach (cards cardington in persistentInventory.cardList)
        {
            GameObject cardObject = Instantiate(cardUIPrefab, cardParent);
            cardUISide ui = cardObject.GetComponent<cardUISide>();
            ui.SetCard(cardington);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PickupItem item))
        {
            if (other.TryGetComponent(out cards card))
            {
                cards copy = card.makeCopy(card);
                player.addCard(copy);
                persistentInventory.addCard(copy);
            }
            if (item.itemName == "potion")
            {
                SceneManager.LoadScene("Card Selector");
            }
            pickup(item);
        }
    }

    public void pickup(PickupItem item)
    {
        inventory.Append(item);
        item.OnPickup();
    }
}

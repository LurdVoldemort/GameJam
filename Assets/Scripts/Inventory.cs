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
    // Start is called before the first frame update
    void Start()
    {
        
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

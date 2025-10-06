using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; // e.g. "Health Potion" or "Coin"

    public void OnPickup()
    {
        Destroy(gameObject); // remove the item from the scene
    }
}

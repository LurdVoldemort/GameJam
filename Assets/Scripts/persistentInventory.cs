using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistentInventory : MonoBehaviour
{
    public static persistentInventory Instance; // Singleton pattern
    public static List<cards> cardList = new List<cards>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void addCard(cards card)
    {
        cardList.Add(card);
    }
}

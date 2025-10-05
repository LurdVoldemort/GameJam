using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cards : MonoBehaviour 
{
    public float length = 1f;
    public float usageRate = 1f;
    public float nextUse = 0f;

    public Sprite cardImage;
    public string cardNameText;
    public string cardDescriptionText;

    public cardUISide cardUIPrefab;

    public cards(float length)
    {
        this.length = length;
    }

    private void Update()
    {
        if (Time.time > nextUse)
        {
            cardUIPrefab.ready();
        }
    }

    public void canUse(Player_Movement player)
    {
        if (Time.time >= nextUse)
        {
            nextUse = Time.time + usageRate;
            this.Activate(player);
        }
    }

    public virtual cards makeCopy(cards card) { return card; }

    public virtual void Activate(Player_Movement player)
    {

    }
}







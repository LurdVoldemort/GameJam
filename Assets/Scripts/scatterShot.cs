using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scatterShot : cards
{

    // Default constructor (length comes from base)
    public scatterShot(float length) : base(length) { }

    // Activate method
    public override void Activate(Player_Movement player)
    {
        // Start the dash coroutine
        StartCoroutine(ScatterCoroutine(player));
    }

    public override cards makeCopy(cards card)
    {
        GameObject cardObject2 = new GameObject("scatterCard");

        scatterShot scatter = cardObject2.AddComponent<scatterShot>();

        scatter.length = card.length;
        scatter.usageRate = card.usageRate;
        scatter.cardImage = card.cardImage;
        scatter.cardNameText = card.cardNameText;

        return scatter;
    }

    private IEnumerator ScatterCoroutine(Player_Movement player)
    {
        player.activeBulletModifier = true;
        cardUIPrefab.used();



        // Wait for duration
        yield return new WaitForSeconds(length);

        // Reset speed
        player.activeBulletModifier = false;
    }
}

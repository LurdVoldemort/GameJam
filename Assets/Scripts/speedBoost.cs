using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBoost : cards
{
    public speedBoost(float speed) : base(speed)
    {
    }
    // Override the base version
    public override void Activate(Player_Movement player)
    {
        // Start a coroutine since we're using yield
        StartCoroutine(BoostCoroutine(player));
    }

    public override cards makeCopy(cards card)
    {
        GameObject cardObject2 = new GameObject("scatterCard");

        speedBoost scatter = cardObject2.AddComponent<speedBoost>();

        scatter.length = card.length;
        scatter.usageRate = card.usageRate;
        scatter.cardImage = card.cardImage;
        scatter.cardNameText = card.cardNameText;

        return scatter;
    }

    private IEnumerator BoostCoroutine(Player_Movement player)
    {
        float originalSpeed = player.speed;
        player.speed = 10f;

        yield return new WaitForSeconds(length);

        // Restore the original speed after time ends
        player.speed = originalSpeed;
    }
}

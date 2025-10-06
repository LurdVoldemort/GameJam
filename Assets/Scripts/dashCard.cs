using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashCard : cards
{

    // Default constructor (length comes from base)
    public dashCard(float length) : base(length) { }

    // Activate method
    public override void Activate(Player_Movement player)
    {
        // Start the dash coroutine
        StartCoroutine(DashCoroutine(player));
    }

    public override cards makeCopy(cards card)
    {
        GameObject cardObject2 = new GameObject("scatterCard");

        dashCard scatter = cardObject2.AddComponent<dashCard>();

        scatter.length = card.length;
        scatter.usageRate = card.usageRate;
        scatter.cardImage = card.cardImage;
        scatter.cardNameText = card.cardNameText;

        return scatter;
    }

    private IEnumerator DashCoroutine(Player_Movement player)
    {
        float originalSpeed = player.speed;

        // Apply dash
        player.speed = 7f;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Ensure we're in 2D plane

        // Direction from player to mouse
        Vector3 dashDirection = (mouseWorldPos - player.transform.position).normalized;

        // Move player instantly in that direction
        player.transform.position += dashDirection * 2;
        cardUIPrefab.used();

        // Wait for duration
        yield return new WaitForSeconds(length);

        // Reset speed
        player.speed = originalSpeed;
    }
}
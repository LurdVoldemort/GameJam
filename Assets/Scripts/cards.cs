using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cards : MonoBehaviour 
{
    public float length = 1f;
    public float usageRate = 1f;
    public float nextUse = 0f;

    public cards(float length)
    {
        this.length = length;
    }

    public void canUse(Player_Movement player)
    {
        if (Time.time >= nextUse)
        {
            nextUse = Time.time + usageRate;
            this.Activate(player);
        }
    }

    public virtual void Activate(Player_Movement player)
    {

    }
}

public class speedBoost : cards
{
    public speedBoost(float speed) : base(speed) { 
    }
    // Override the base version
    public override void Activate(Player_Movement player)
    {
        // Start a coroutine since we're using yield
        StartCoroutine(BoostCoroutine(player));
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

        // Wait for duration
        yield return new WaitForSeconds(length);

        // Reset speed
        player.speed = originalSpeed;
    }
}

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

    private IEnumerator ScatterCoroutine(Player_Movement player)
    {
        player.activeBulletModifier = true;
        Debug.Log("I am here" + player.activeBulletModifier);


        // Wait for duration
        yield return new WaitForSeconds(length);

        // Reset speed
        player.activeBulletModifier = false;
    }
}

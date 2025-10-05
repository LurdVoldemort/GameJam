using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Assign your PauseMenu Canvas here in the inspector
    public static bool GameIsPaused = false;
    public List<cards> allCards;
    public List<Sprite> allSprites;
    private Transform spawnPoint;
    private List<string> cardNames = new List<string>()
{
    "Flame Strike",
    "Healing Touch",
    "Shadow Blade",
    "Lightning Bolt",
    "Arcane Shield",
    "Frost Nova",
    "Wind Gust",
    "Earthquake",
    "Divine Light",
    "Dark Pact",
    "Phoenix Fire",
    "Mystic Barrier",
    "Stormcaller",
    "Vampiric Touch",
    "Time Warp",
    "Gravity Well",
    "Soul Drain",
    "Celestial Blessing",
    "Nightfall",
    "Inferno Blast"
};
    private cards randomCard;

    private void Start()
    {
        GameObject obj = GameObject.Find("CardSpawnPoint");
        if (obj != null)
        {
            spawnPoint = obj.transform;
        }
        StartCoroutine(SpawnCardLoop());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Press ESC to pause
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private IEnumerator SpawnCardLoop()
    {
        while (true)
        {
            SpawnCard();
            yield return new WaitForSeconds(5f);
        }
    }

    private void SpawnCard()
    {
        if (allCards.Count == 0)
        {
            Debug.LogWarning("No cards in allCards list!");
            return;
        }

        int index;
        float roll = Random.value; // gives a random float between 0.0 and 1.0

        if (roll < 0.6f)
        {
            // 60% chance: pick the last 
            index = allCards.Count - 1;
            randomCard = allCards[index];
            randomCard.cardImage = allSprites[Random.Range(0, allSprites.Count)];
            randomCard.cardNameText = cardNames[Random.Range(0, cardNames.Count)];

        }
        else
        {
            // 40% chance: pick from all others (except the last)
            index = Random.Range(0, allCards.Count - 1);
            randomCard = allCards[index];
        }

        float xOffset = Random.Range(-1f, 1f); // random between -1 and 1 units
        float yOffset = Random.Range(-1f, 1f);
        Vector3 spawnPos = spawnPoint.position + new Vector3(xOffset, yOffset, 0f);

        // Spawn it at the CardSpawnPoint
        Instantiate(randomCard, spawnPos, Quaternion.identity);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze game time
        GameIsPaused = true;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

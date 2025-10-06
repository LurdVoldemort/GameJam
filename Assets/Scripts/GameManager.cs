using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawn;
    [SerializeField] public float maxEnemiesInPlay;
    private Player_Movement player_Movement;
    public int spawnersInPlay;
    public int enemiesInPlay;

    public GameObject pauseMenuUI;
    public GameObject killEnemiesReminder;
    public static bool GameIsPaused = false;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this instance across scenes
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Press ESC to pause
        {
            if (GameIsPaused) { Resume(); }
            else { Pause(); }
        }

        if (SceneManager.GetActiveScene().name == "Room 1") //basically update() for room 1
        {
            if (spawnersInPlay <= 0)
            {
                //pop up ui that says "kill remaining enemies
                killEnemiesReminder.SetActive(true);
                if (enemiesInPlay <= 0)
                {
                    //SceneManager.LoadScene("Card Selector");
                }
            }
        }
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

    public void PlayerDied(GameObject player)
    {
        //sound effect

        //rotate sprite 90
        player.transform.Rotate(0, 0, 90);

        //colour red
        player.GetComponent<SpriteRenderer>().color = Color.red;

        StartCoroutine(WaitABit(2f));
        //go to next scene
        SceneManager.LoadScene("GameOver");
    }

    public void OnStartButtonClick()
    {
        // Subscribe to the event before loading
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Room 1");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Room 1") // basically start() for room 1
        {
            spawnersInPlay = GameObject.FindGameObjectsWithTag("Spawner").Length;
            enemiesInPlay = GameObject.FindGameObjectsWithTag("MeleeEnemy").Length + GameObject.FindGameObjectsWithTag("RangedEnemy").Length;
            //find pause menu
            pauseMenuUI = GameObject.Find("PauseScreen");
            pauseMenuUI.SetActive(false);

            killEnemiesReminder = GameObject.Find("KillEnemiesUI");
            killEnemiesReminder.SetActive(false);

            //find playerspawn
            playerSpawn = GameObject.Find("PlayerSpawn").transform;

            // Spawn the player in Room 1
            Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);

            // Unsubscribe so this only runs once
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public IEnumerator WaitABit(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}

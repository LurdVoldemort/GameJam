using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cutsceneManager : MonoBehaviour
{
    public float frameDuration = 100f;          // How long each frame is shown
    public string nextSceneName = "SampleScene"; // Scene to load after the cutscene

    private List<GameObject> frames = new List<GameObject>();
    private int currentFrame = 0;

    void Start()
    {
        // Gather all child frames under this GameObject
        foreach (Transform child in transform)
        {
            frames.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        if (frames.Count > 0)
            StartCoroutine(PlayCutscene());
        else
            Debug.LogWarning("No frames found under Cutscene object!");
    }

    IEnumerator PlayCutscene()
    {
        while (currentFrame < frames.Count)
        {
            // Show only the current frame
            for (int i = 0; i < frames.Count; i++)
                frames[i].SetActive(i == currentFrame);

            // Wait for the duration (or skip on input)
            float timer = 0f;
            while (timer < frameDuration)
            {
                if (Input.anyKeyDown)
                {
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }

            currentFrame++;
        }

        // Hide all frames at the end
        foreach (var frame in frames)
            frame.SetActive(false);

        // Optionally load next scene
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}



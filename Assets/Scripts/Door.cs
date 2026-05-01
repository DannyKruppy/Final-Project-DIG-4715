using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour
{
    [Header("Scene Transition")]
    [SerializeField] private string sceneName;
    [SerializeField] private Image blackScreen;

    [Header("Door Type")]
    public bool isHubDoor = false;

    [Header("Timer (Level Doors Only)")]
    public Timer timer;

    [Header("Hub Settings")]
    public string[] levelNames;
    public float[] maxTimes;

    public string winScene = "StoryWin";
    public string loseScene = "StoryLose";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        triggered = true;

        // =====================================
        // NORMAL LEVEL DOOR
        // =====================================
        if (!isHubDoor)
        {
            if (timer != null)
            {
                timer.CompleteLevel();
            }

            StartCoroutine(FadeToBlack(sceneName));
            return;
        }

        // =====================================
        // HUB FINAL DOOR
        // =====================================

        // Check completion
        if (!AllLevelsCompleted())
        {
            Debug.Log("Not all levels completed!");

            triggered = false;
            return;
        }

        bool passed = AllLevelsUnderTime();
        Debug.Log("Final Result = " + passed);

        // Check win/loss
        string targetScene =
            passed
            ? winScene
            : loseScene;

        Debug.Log("Loading Scene: " + targetScene);

        StartCoroutine(FadeToBlack(targetScene));
    }

    // =====================================
    // CHECK ALL LEVELS COMPLETED
    // =====================================
    bool AllLevelsCompleted()
    {
        foreach (string lvl in levelNames)
        {
            if (!GameSession.HasLevel(lvl))
            {
                return false;
            }
        }

        return true;
    }

    // =====================================
    // CHECK ALL TIMES UNDER LIMIT
    // =====================================
    bool AllLevelsUnderTime()
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            float time =
                GameSession.GetLevelTime(levelNames[i]);

            // Missing level
            if (time < 0)
            {
                return false;
            }

            // Failed time requirement
            if (time > maxTimes[i])
            {
                Debug.Log(
                    levelNames[i] +
                    " FAILED | Time: " +
                    time +
                    " | Max: " +
                    maxTimes[i]
                );

                return false;
            }

            Debug.Log(
                levelNames[i] +
                " PASSED | Time: " +
                time +
                " | Max: " +
                maxTimes[i]
            );
        }

        return true;
    }

    // =====================================
    // FADE TRANSITION
    // =====================================
    private IEnumerator FadeToBlack(string targetScene)
    {
        float t = 0f;

        Color imageColor = blackScreen.color;

        while (t < 1f)
        {
            t += Time.deltaTime;

            imageColor.a = t;

            blackScreen.color = imageColor;

            yield return null;
        }

        SceneManager.LoadScene(targetScene);
    }
}
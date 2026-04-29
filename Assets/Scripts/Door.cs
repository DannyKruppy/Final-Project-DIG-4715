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
    public string winScene = "WinScene";
    public string loseScene = "GameOver";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // =========================
        // LEVEL DOOR
        // =========================
        if (!isHubDoor)
        {
            if (timer != null)
            {
                timer.CompleteLevel();
            }

            StartCoroutine(FadeToBlack(sceneName));
            return;
        }

        // =========================
        // HUB DOOR
        // =========================
        if (!AllLevelsCompleted())
        {
            Debug.Log("Not all levels completed yet!");
            triggered = false;
            return;
        }

        string targetScene = AllLevelsUnderTime() ? winScene : loseScene;

        StartCoroutine(FadeToBlack(targetScene));
    }

    // =========================
    // CHECK COMPLETION
    // =========================
    bool AllLevelsCompleted()
    {
        foreach (string lvl in levelNames)
        {
            if (!PlayerPrefs.HasKey(lvl + "_BestTime"))
                return false;
        }
        return true;
    }

    // =========================
    // CHECK TIMES
    // =========================
    bool AllLevelsUnderTime()
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            float time = PlayerPrefs.GetFloat(levelNames[i] + "_BestTime", float.MaxValue);

            if (time > maxTimes[i])
                return false;
        }
        return true;
    }

    // =========================
    // FADE + LOAD
    // =========================
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
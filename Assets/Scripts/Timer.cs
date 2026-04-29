using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    private float count;
    public float maxCount = 100;

    public GameObject timerBar;
    private Image timerBarImage;

    public TextMeshProUGUI timerText;

    [Header("UI Colors")]
    public Color normalColor = Color.white;
    public Color overtimeColor = Color.red;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip warningTickSound;

    private bool warningStarted = false;
    private Coroutine warningCoroutine;

    [Header("Level Info")]
    public string levelName; // e.g. "Lvl1", "Lvl2", etc.

    void Start()
    {
        timerBarImage = timerBar.GetComponent<Image>();
        resetTimer();
    }

    void Update()
    {
        count -= Time.deltaTime;

        // Start warning ticks near time limit
        if (count <= 10f && !warningStarted)
        {
            warningStarted = true;
            warningCoroutine = StartCoroutine(WarningTickRoutine());
        }

        // Overtime visual (count < 0)
        if (count < 0)
        {
            timerText.color = overtimeColor;
        }
        else
        {
            timerText.color = normalColor;
        }

        if (count <= 0)
        {
            // Stop ticking sound instantly
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }

            audioSource.Stop();
        }

        // UI display (show real time remaining / overtime)
        timerBarImage.fillAmount = Mathf.Clamp01(count / maxCount);
        timerText.text = Mathf.Abs(count).ToString("F2");
    }

    public void resetTimer()
    {
        count = maxCount;
        warningStarted = false;
        timerText.color = normalColor;
    }

    // =========================
    // CALL WHEN PLAYER COMPLETES LEVEL
    // =========================
    public void CompleteLevel()
    {
        // TRUE TIME TAKEN (even if overtime)
        float timeTaken = Mathf.Max(0, maxCount - count);

        SaveLevelTime(timeTaken);
    }

    // =========================
    // SAVE TIME PER LEVEL
    // =========================
    void SaveLevelTime(float timeTaken)
    {
        string key = levelName + "_Time";

        // Always overwrite (we want THIS run, not best run)
        PlayerPrefs.SetFloat(key, timeTaken);
        PlayerPrefs.Save();
    }

    // =========================
    // HUB CHECK SYSTEM (call this from Hub)
    // =========================
    public static bool HasCompletedAllLevels(string[] levelNames)
    {
        foreach (string lvl in levelNames)
        {
            if (!PlayerPrefs.HasKey(lvl + "_Time"))
                return false;
        }
        return true;
    }

    public static bool AllLevelsUnderTime(string[] levelNames, float[] maxTimes)
    {
        for (int i = 0; i < levelNames.Length; i++)
        {
            float time = PlayerPrefs.GetFloat(levelNames[i] + "_Time", float.MaxValue);

            if (time > maxTimes[i])
                return false;
        }
        return true;
    }

    private IEnumerator WarningTickRoutine()
    {
        while (count > 0)
        {
            audioSource.PlayOneShot(warningTickSound);

            float waitTime = count <= 5f ? 0.5f : 1f;
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
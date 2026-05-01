using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour
{
    private float count;
    public float maxCount = 100;

    [Header("UI")]
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
    public string levelName;

    private bool levelCompleted = false;

    void Start()
    {
        timerBarImage = timerBar.GetComponent<Image>();
        ResetTimer();
    }

    void Update()
    {
        // Stop timer after completion
        if (!levelCompleted)
        {
            count -= Time.deltaTime;
        }

        // Start warning ticks
        if (count <= 10f && count > 0 && !warningStarted)
        {
            warningStarted = true;
            warningCoroutine = StartCoroutine(WarningTickRoutine());
        }

        // Overtime color
        timerText.color =
            (count < 0) ? overtimeColor : normalColor;

        // Stop ticking after timer passes 0
        if (count <= 0 && warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
            warningCoroutine = null;

            audioSource.Stop();
        }

        // UI
        timerBarImage.fillAmount =
            Mathf.Clamp01(count / maxCount);

        timerText.text =
            Mathf.Abs(count).ToString("F2");
    }

    public void ResetTimer()
    {
        count = maxCount;

        warningStarted = false;
        levelCompleted = false;

        timerText.color = normalColor;
    }

    // =====================================
    // LEVEL COMPLETE
    // =====================================
    public void CompleteLevel()
    {
        if (levelCompleted) return;

        levelCompleted = true;

        // Real time taken
        float timeTaken =
            Mathf.Max(0, maxCount - count);

        // Save THIS RUN
        GameSession.SaveLevelTime(levelName, timeTaken);

        // Save permanent best
        SaveBestTime(timeTaken);

        Debug.Log(
            "RUN SAVED: " +
            levelName +
            " = " +
            timeTaken
        );
    }

    // =====================================
    // SAVE BEST TIME (PERMANENT)
    // =====================================
    void SaveBestTime(float timeTaken)
    {
        string key = levelName + "_BestTime";

        float currentBest =
            PlayerPrefs.GetFloat(key, float.MaxValue);

        // First run OR better run
        if (timeTaken < currentBest)
        {
            PlayerPrefs.SetFloat(key, timeTaken);
            PlayerPrefs.Save();

            Debug.Log("NEW BEST TIME: " + timeTaken);
        }
    }

    // =====================================
    // WARNING TICKS
    // =====================================
    private IEnumerator WarningTickRoutine()
    {
        while (count > 0 && !levelCompleted)
        {
            audioSource.PlayOneShot(warningTickSound);

            // Faster ticks near 0
            float waitTime =
                (count <= 5f) ? 0.5f : 1f;

            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
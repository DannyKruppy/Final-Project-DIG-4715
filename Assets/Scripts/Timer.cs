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
    public string levelName; // e.g. "Level1", "Level2", "Level3"

    private bool levelCompleted = false;

    void Start()
    {
        timerBarImage = timerBar.GetComponent<Image>();
        resetTimer();
    }

    void Update()
    {
        if (!levelCompleted)
            count -= Time.deltaTime;

        // Warning ticks
        if (count <= 10f && count > 0 && !warningStarted)
        {
            warningStarted = true;
            warningCoroutine = StartCoroutine(WarningTickRoutine());
        }

        // Color change for overtime
        timerText.color = (count < 0) ? overtimeColor : normalColor;

        // Stop audio at 0
        if (count <= 0 && warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
            warningCoroutine = null;
            audioSource.Stop();
        }

        timerBarImage.fillAmount = Mathf.Clamp01(count / maxCount);
        timerText.text = Mathf.Abs(count).ToString("F2");
    }

    public void resetTimer()
    {
        count = maxCount;
        warningStarted = false;
        levelCompleted = false;
        timerText.color = normalColor;
    }

    // =========================
    // CALL WHEN LEVEL IS COMPLETED
    // =========================
    public void CompleteLevel()
    {
        if (levelCompleted) return;

        levelCompleted = true;

        float timeTaken = Mathf.Max(0, maxCount - count);

        SaveLevelTime(timeTaken);

        Debug.Log("Saved: " + levelName + "_BestTime = " + timeTaken);

        Debug.Log(
            levelName +
            " | TimeTaken: " + timeTaken +
            " | MaxAllowed: " + maxCount +
            " | Raw Count: " + count
        );
    }

    // =========================
    // SAVE BEST TIME
    // =========================
    void SaveLevelTime(float timeTaken)
    {
        string key = levelName + "_BestTime";

        float currentBest = PlayerPrefs.GetFloat(key, float.MaxValue);

        if (timeTaken < currentBest)
        {
            PlayerPrefs.SetFloat(key, timeTaken);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator WarningTickRoutine()
    {
        while (count > 0 && !levelCompleted)
        {
            audioSource.PlayOneShot(warningTickSound);

            float waitTime = count <= 5f ? 0.5f : 1f;
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }
}
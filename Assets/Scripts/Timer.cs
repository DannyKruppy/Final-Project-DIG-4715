using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    private float count;
    public float maxCount = 100;

    public GameObject timerBar;
    private Image timerBarImage;

    public TextMeshProUGUI timerText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gameOverSound;
    public AudioClip warningTickSound;

    private bool warningStarted = false;

    private bool gameOverTriggered = false;

    private Coroutine warningCoroutine;
    
    [Header("Player Movement Script")]
    public CharacterControllerScript playerMovement;

    [Header("Level Info")]
    public string levelName; // "Level1", "Level2", "Level3"

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerBarImage = timerBar.GetComponent<Image>();
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverTriggered) return; 
        count -= Time.deltaTime;

        if (count <= 10f && count > 0 && !warningStarted)
        {
            warningStarted = true;
            warningCoroutine = StartCoroutine(WarningTickRoutine());
        }

        if (count <= 0)
        {
            count = 0;
            gameOverTriggered = true;
            playerMovement.enabled = false;

            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
                warningCoroutine = null;
            }

            audioSource.Stop(); // Stop tick sound

            audioSource.PlayOneShot(gameOverSound); // Play Game Over sound
            StartCoroutine(GameOverRoutine());
        }

        timerBarImage.fillAmount = count / maxCount;
        timerText.text = count.ToString("F2");
    }

    public void resetTimer()
    {
        count = maxCount;
        gameOverTriggered = false;
        warningStarted = false;
    }

    // When Player Wins
    public void CompleteLevel()
    {
        float timeTaken = maxCount - count;

        SaveLevelTime(timeTaken);

        if (levelName == "Level3") { CheckAndSaveTotalTime(); }
    }

    // Save Best Time Per Level
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

    // Total Best Time (Highscore) System
    void CheckAndSaveTotalTime()
    {
        float t1 = PlayerPrefs.GetFloat("Level1_BestTime", float.MaxValue);
        float t2 = PlayerPrefs.GetFloat("Level2_BestTime", float.MaxValue);
        float t3 = PlayerPrefs.GetFloat("Level3_BestTime", float.MaxValue);

        if (t1 == float.MaxValue || t2 == float.MaxValue || t3== float.MaxValue) { return; }

        float totalTime = t1 + t2 + t3;

        float bestTotal = PlayerPrefs.GetFloat("TotalBestTime", float.MaxValue);

        if (totalTime < bestTotal)
        {
            PlayerPrefs.SetFloat("TotalBestTime", totalTime);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator WarningTickRoutine()
    {
        while (count > 0 && count <= 10f)
        {
            audioSource.PlayOneShot(warningTickSound);

            // Faster ticking in last 5 seconds
            float waitTime = count <= 5f ? 0.5f : 1f;
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(2f); // Delay before loading scene
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
    }
}

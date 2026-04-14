using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;

public class Timer : MonoBehaviour
{
    private float count;
    public float maxCount = 100;

    public GameObject timerBar;
    private Image timerBarImage;

    public TextMeshProUGUI timerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerBarImage = timerBar.GetComponent<Image>();
        resetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        timerBarImage.fillAmount = count / maxCount;
        timerText.text = count.ToString("F2");
    }

    public void resetTimer()
    {
        count = maxCount;
    }
}

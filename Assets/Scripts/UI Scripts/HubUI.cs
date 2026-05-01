using TMPro;
using UnityEngine;

public class HubUI : MonoBehaviour
{
    [Header("3D Text Objects")]
    public TextMeshPro level1Text;
    public TextMeshPro level2Text;
    public TextMeshPro level3Text;
    public TextMeshPro totalText;

    [Header("Level Names")]
    public string[] levelNames;

    [Header("Max Time Limits")]
    public float[] maxTimes;

    void Start()
    {
        // Get CURRENT RUN times
        float t1 = GameSession.GetLevelTime(levelNames[0]);
        float t2 = GameSession.GetLevelTime(levelNames[1]);
        float t3 = GameSession.GetLevelTime(levelNames[2]);

        // Total runtime
        float total = 0f;

        if (t1 >= 0) total += t1;
        if (t2 >= 0) total += t2;
        if (t3 >= 0) total += t3;

        // Update UI
        SetLevelText(level1Text, levelNames[0], t1, maxTimes[0]);
        SetLevelText(level2Text, levelNames[1], t2, maxTimes[1]);
        SetLevelText(level3Text, levelNames[2], t3, maxTimes[2]);

        totalText.text =
            total > 0
            ? "Total: " + total.ToString("F2") + "s"
            : "Total: --";
    }

    // =====================================
    // SET TEXT + COLOR
    // =====================================
    void SetLevelText(
        TextMeshPro textObj,
        string label,
        float time,
        float maxTime)
    {
        // Level not completed
        if (time < 0)
        {
            textObj.text = label + ": --";
            textObj.color = Color.white;
            return;
        }

        // Display time
        textObj.text =
            label + ": " +
            time.ToString("F2") + "s";

        // GOOD RUN
        if (time <= maxTime)
        {
            textObj.color = Color.green;
        }
        // FAILED RUN
        else
        {
            textObj.color = Color.red;
        }
    }
}
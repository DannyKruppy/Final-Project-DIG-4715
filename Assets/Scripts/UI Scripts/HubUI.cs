using TMPro;
using UnityEngine;

public class HubUI : MonoBehaviour
{
    public TextMeshPro level1Text;
    public TextMeshPro level2Text;
    public TextMeshPro level3Text;
    public TextMeshPro totalText;

    [Header("Max Time Limits Per Level")]
    public float[] maxTimes = new float[3];

    void Start()
    {
        float t1 = PlayerPrefs.GetFloat("Level1_BestTime", -1f);
        float t2 = PlayerPrefs.GetFloat("Level2_BestTime", -1f);
        float t3 = PlayerPrefs.GetFloat("Level3_BestTime", -1f);
        float total = PlayerPrefs.GetFloat("TotalBestTime", -1f);

        SetLevelText(level1Text, "Level 1", t1, maxTimes[0]);
        SetLevelText(level2Text, "Level 2", t2, maxTimes[1]);
        SetLevelText(level3Text, "Level 3", t3, maxTimes[2]);
        totalText.text = FormatTime("Total", total);
    }

    void SetLevelText(TextMeshPro textObj, string label, float time, float maxTime)
    {
        textObj.text = FormatTime(label, time);

        if (time == null || time < 0)
        {
            textObj.color = Color.white; // Level not completed yet
            return;
        }

        // Green = under limit (good run)
        if (time <= maxTime)
        {
            textObj.color = Color.green;
        }
        // Red = over limit (bad run)
        else
        {
            textObj.color = Color.red;
        }


    }

    string FormatTime(string label, float time)
    {
        if (time < 0)
            return label + ": --";

        return label + ": " + time.ToString("F2") + "s";
    }
}
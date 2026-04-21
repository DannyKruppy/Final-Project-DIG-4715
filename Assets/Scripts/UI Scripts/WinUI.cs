using TMPro;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public TextMeshProUGUI level1Text;
    public TextMeshProUGUI level2Text;
    public TextMeshProUGUI level3Text;
    public TextMeshProUGUI totalText;

    void Start()
    {
        float t1 = PlayerPrefs.GetFloat("Level1_BestTime", -1f);
        float t2 = PlayerPrefs.GetFloat("Level2_BestTime", -1f);
        float t3 = PlayerPrefs.GetFloat("Level3_BestTime", -1f);
        float total = PlayerPrefs.GetFloat("TotalBestTime", -1f);

        level1Text.text = FormatTime("Level 1", t1);
        level2Text.text = FormatTime("Level 2", t2);
        level3Text.text = FormatTime("Level 3", t3);
        totalText.text = FormatTime("Total", total);
    }

    string FormatTime(string label, float time)
    {
        if (time < 0)
            return label + ": --";

        return label + ": " + time.ToString("F2") + "s";
    }
}
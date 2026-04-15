using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private Image blackScreen;

    void Start()
    {
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        float t = 0f;
        Color imageColor = blackScreen.color;

        while (t < 1f)
        {
            t += Time.deltaTime;
            imageColor.a = 1f - (t / 1f);
            blackScreen.color = imageColor;
            yield return null;
        }
    }
}

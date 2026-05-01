using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class HubTeleportDoors : MonoBehaviour
{
    [Header("Scene To Load")]
    public string targetScene;

    [Header("Fade Settings")]
    public Image blackScreen;
    public float fadeDuration = 1f;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color color = blackScreen.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = t / fadeDuration;
            blackScreen.color = color;
            yield return null;
        }

        SceneManager.LoadScene(targetScene);
    }
}
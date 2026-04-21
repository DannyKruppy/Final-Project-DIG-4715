using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Door : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Image blackScreen;

    public Timer timer;
    private bool triggered = false; // for preventing double trigger

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) { return; }

        if(other.tag == "Player" && sceneName != null)
        {
            triggered = true;

            // Level Completed -> Time Save
            if (timer != null) { timer.CompleteLevel(); }

            StartCoroutine(FadeToBlack());
        }

    }

    private IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color imageColor = blackScreen.color;

        while (t < 1f)
        {
            t += Time.deltaTime;
            imageColor.a = t / 1f;
            blackScreen.color = imageColor;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}

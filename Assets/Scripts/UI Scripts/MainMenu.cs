using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;

    public TitleMusic titleMusic;

    [Header("How To Play UI")]
    public GameObject keyboardImage;
    public GameObject controllerImage;
    public GameObject mechanicsPanel;
    public GameObject obstaclesPanel;

    private GameObject currentOpenPanel;

    private void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    public void PlayGame()
    {
        if (titleMusic != null)
        {
            titleMusic.StopMusic();
        }

        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Level1Blockout"));
    }

    public void LevelOne()
    {
        if (titleMusic != null)
        {
            titleMusic.StopMusic();
        }

        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Level1Blockout"));
    }

    public void LevelSelect()
    {
        PlayClick();
        StartCoroutine(LoadSceneWithDelay("LevelSelect"));
    }

    public void QuitGame()
    {
        PlayClick();
        StartCoroutine(QuitRoutine());
    }

    public void Credits()
    {
        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Credits"));
    }

    public void Controls()
    {
        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Controls"));
    }

    public void Lore()
    {
        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Lore"));
    }

    public void BacktoMain()
    {
        PlayClick();
        StartCoroutine(LoadSceneWithDelay("Title"));
    }

    private IEnumerator LoadSceneWithDelay(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.2f); // small delay so sound plays
        SceneManager.LoadSceneAsync(sceneName);
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Application.Quit();
    }

    private void ToggleSection(GameObject target)
    {
        // If clicking the same one -> toggle off
        if (currentOpenPanel == target && target.activeSelf)
        {
            target.SetActive(false);
            currentOpenPanel = null;
            return;
        }

        // Hide everything first
        keyboardImage.SetActive(false);
        controllerImage.SetActive(false);
        mechanicsPanel.SetActive(false);
        obstaclesPanel.SetActive(false);

        // Show selected
        target.SetActive(true);
        currentOpenPanel = target;
    }

    public void ShowKeyboard()
    {
        PlayClick();
        ToggleSection(keyboardImage);
    }

    public void ShowController()
    {
        PlayClick();
        ToggleSection(controllerImage);
    }

    public void ShowMechanics()
    {
        PlayClick();
        ToggleSection(mechanicsPanel);
    }

    public void ShowObstacles()
    {
        PlayClick();
        ToggleSection(obstaclesPanel);
    }
}
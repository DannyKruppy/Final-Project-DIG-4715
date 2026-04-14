using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseScript : MonoBehaviour
{
    public InputAction pauseAction;

    public bool paused = false;
    public GameObject pauseMenu;
    public GameObject gameCanvas;

    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;

    public AudioSource musicSource;

    void OnEnable()
    {
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        gameCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;

        if (paused)
        {
            pauseMenu.SetActive(true);
            gameCanvas.SetActive(false);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            pauseMenu.SetActive(false);
            gameCanvas.SetActive(true);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // This is just for hooking to the Resume Button so that the sound can be played only when the button's pressed and not when you unpause using ESC
    public void Resume()
    {
        PlayClick();
        TogglePause();
    }

    public void ExitToMenu()
    {
        PlayClick();
        StartCoroutine(ExitToMenuRoutine());

    }

    private IEnumerator ExitToMenuRoutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadSceneAsync("Title");
    }

    public void ExitToDesktop()
    {
        PlayClick();
        StartCoroutine(QuitRoutine());
    }

    private void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Application.Quit();
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }
}

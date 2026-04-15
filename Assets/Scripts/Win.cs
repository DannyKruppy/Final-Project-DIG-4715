using UnityEngine;

public class Win : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip WinSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.PlayOneShot(WinSFX);
    }

}

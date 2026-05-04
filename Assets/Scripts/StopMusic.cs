using UnityEngine;

public class StopMusic : MonoBehaviour
{
    void Start()
    {
        var music = FindAnyObjectByType<TitleMusic>();

        if (music != null)
        {
            Destroy(music.gameObject);
        }
    }
}
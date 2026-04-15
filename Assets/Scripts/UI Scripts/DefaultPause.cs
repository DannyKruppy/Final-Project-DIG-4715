using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultPause : MonoBehaviour
{
    public GameObject firstButton;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void OnDisable()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

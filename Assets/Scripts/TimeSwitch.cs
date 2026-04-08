using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class TimeSwitch : MonoBehaviour
{
    public GameObject[] pastObjects;
    public GameObject[] futureObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject obj in futureObjects)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            foreach (GameObject obj in pastObjects)
            {
                obj.SetActive(!obj.activeSelf);
            }
            foreach (GameObject obj2 in futureObjects)
            {
                obj2.SetActive(!obj2.activeSelf);
            }
        }
    }
}

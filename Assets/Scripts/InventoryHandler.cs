using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler Instance { get; private set; }

    public bool blueKey;
    public bool orangeKey;
    public bool pinkKey;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler Instance { get; private set; }

    private bool blueKey;
    private bool orangeKey;
    private bool pinkKey;

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

    public void BlueKeySet(bool b)
    {
        blueKey = b;
    }

    public bool BlueKeyGet()
    {
        return(blueKey);
    }

    public void OrangeKeySet(bool b)
    {
        orangeKey = b;
    }

    public bool OrangeKeyGet()
    {
        return (orangeKey);
    }

    public void PinkKeySet(bool b)
    {
        pinkKey = b;
    }

    public bool PinkKeyGet()
    {
        return (pinkKey);
    }
}

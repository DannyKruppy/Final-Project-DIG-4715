using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    [SerializeField] private string keyColor;
    [SerializeField] private Sprite keyFull;
    [SerializeField] private Sprite keyEmpty;

    private Image myImageComponent;
    private InventoryHandler inventoryHandler;

    private void Start()
    {
        myImageComponent = this.GetComponent<Image>();
        inventoryHandler = InventoryHandler.Instance;
        UpdateUI();
    }

    public void UpdateUI()
    {
        switch (keyColor.ToLower())
        {
            case "blue":
                if (inventoryHandler.BlueKeyGet())
                {
                    myImageComponent.sprite = keyFull;
                    myImageComponent.color = Color.blue;
                }
                else
                {
                    myImageComponent.sprite = keyEmpty;
                    myImageComponent.color = Color.blue;
                }
                    break;
            case "orange":
                if (inventoryHandler.OrangeKeyGet())
                {
                    myImageComponent.sprite = keyFull;
                    myImageComponent.color = Color.orange;
                }
                else
                {
                    myImageComponent.sprite = keyEmpty;
                    myImageComponent.color = Color.orange;
                }
                break;
            case "pink":
                if (inventoryHandler.PinkKeyGet())
                {
                    myImageComponent.sprite = keyFull;
                    myImageComponent.color = Color.hotPink;
                }
                else
                {
                    myImageComponent.sprite = keyEmpty;
                    myImageComponent.color = Color.hotPink;
                }
                break;
        }
    }
}

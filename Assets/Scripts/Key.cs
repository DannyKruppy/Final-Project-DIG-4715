using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyColor;
    [SerializeField] private KeyUI keyUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pickupSound;

    private InventoryHandler inventoryHandler;

    private void Start()
    {
        inventoryHandler = InventoryHandler.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(pickupSound);

        if(keyColor != null)
        {
            switch (keyColor.ToLower())
            {
                case "blue":
                    inventoryHandler.BlueKeySet(true);
                    keyUI.UpdateUI();
                    break;
                case "orange":
                    inventoryHandler.OrangeKeySet(true);
                    keyUI.UpdateUI();
                    break;
                case "pink":
                    inventoryHandler.PinkKeySet(true);
                    keyUI.UpdateUI();
                    break;
            }
        }

        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private string keyColor;
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
                    inventoryHandler.blueKey = true;
                    break;
                case "orange":
                    inventoryHandler.orangeKey = true;
                    break;
                case "pink":
                    inventoryHandler.pinkKey = true;
                    break;
            }
        }

        Destroy(gameObject);
    }
}

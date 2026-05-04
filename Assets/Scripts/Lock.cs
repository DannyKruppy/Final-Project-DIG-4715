using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private string lockColor;
    [SerializeField] private GameObject lockModel;
    [SerializeField] private ParticleSystem explosionEffect;

    private InventoryHandler inventoryHandler;

    private void Start()
    {
        inventoryHandler = InventoryHandler.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            switch (lockColor.ToLower())
            {
                case "blue":
                    if (inventoryHandler.BlueKeyGet())
                    {
                        //explosion and lock removal
                        lockModel.SetActive(false);
                        explosionEffect.Play();
                        Invoke("selfDestruct", 1);
                    }
                        break;
                case "orange":
                    if (inventoryHandler.OrangeKeyGet())
                    {
                        //explosion and lock removal
                        lockModel.SetActive(false);
                        explosionEffect.Play();
                        Invoke("selfDestruct", 1);
                    }
                        break;
                case "pink":
                    if (inventoryHandler.PinkKeyGet())
                    {
                        //explosion and lock removal
                        lockModel.SetActive(false);
                        explosionEffect.Play();
                        Invoke("selfDestruct", 1);
                    }
                        break;
            }
        }
    }

    private void selfDestruct()
    {
        Destroy(gameObject);
    }
}
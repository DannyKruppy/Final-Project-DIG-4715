using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    public SwitchWallController controller;
    public bool playerOnSwitch = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnSwitch)
        {
            playerOnSwitch = true;
            controller.ActivateSwitch(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnSwitch = false;
        }
    }
}
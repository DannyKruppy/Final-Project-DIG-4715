using UnityEngine;

public class SwitchTrigger : MonoBehaviour
{
    public SwitchWallController controller;
    public bool playerOnSwitch = false;

    public AudioSource audioSource;
    public AudioClip switchSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerOnSwitch)
        {
            playerOnSwitch = true;
            controller.ActivateSwitch(gameObject);
            audioSource.PlayOneShot(switchSound);
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
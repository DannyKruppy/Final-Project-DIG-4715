using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject doorLock;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(pickupSound);
        Destroy(doorLock);
        Destroy(gameObject);
    }
}

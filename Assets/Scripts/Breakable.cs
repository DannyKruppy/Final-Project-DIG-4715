using UnityEngine;
using UnityEngine.UI;

public class Breakable : MonoBehaviour
{
    private float hp = 1f;
    public float maxHp = 1f;

    public Image indicator;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            return;
        }

        float normalizedHp = hp / maxHp;
        indicator.fillAmount = 1f - normalizedHp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Triggered with Player!");
            hp -= Time.deltaTime;
        }
    }
}
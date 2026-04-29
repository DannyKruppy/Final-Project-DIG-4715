using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Breakable : MonoBehaviour
{
    public float hp = 1f;
    public float maxHp = 1f;

    public GameObject platform;
    public Image indicator;

    private bool isBroken = false;

    void Start()
    {
        hp = maxHp;
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0f, maxHp);
        if (!isBroken)
        {
            float normalizedHp = hp / maxHp;
            indicator.fillAmount = 1f - normalizedHp;

            if (hp <= 0)
            {
                StartCoroutine(Respawn());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hp -= Time.deltaTime;
        }
        else
        {
            hp += Time.deltaTime / 8;
        }
    }

    private IEnumerator Respawn()
    {
        isBroken = true;

        platform.SetActive(false);

        yield return new WaitForSeconds(5f);

        hp = maxHp;
        platform.SetActive(true);

        isBroken = false;
    }
}
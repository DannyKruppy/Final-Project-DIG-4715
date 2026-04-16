using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class TimeSwitch : MonoBehaviour
{
    public InputAction switchAction;

    public AudioSource audioSource;

    public AudioClip startSound;
    public AudioClip endSound;

    public GameObject[] pastObjects;
    public GameObject[] futureObjects;
    public GameObject[] persistentObjects;

    public Material pastMat;
    public Material futureMat;
    public Material persistentMat;

    // JADON
    public GameObject player;
    public Material playerPastMat;
    public Material playerFutureMat;

    public PauseScript pauseManager;

    bool isPast = true;
    bool timeLocked = false;

    public float maxTime = 8f;
    public float drainRate = 1f;
    public float rechargeRate = 0.5f;
    public float timeDelay = 2f;

    private float time;
    private float counter;

    public GameObject timeBar;
    private Image timeBarImage;

    Color blueColor;
    Color redColor;

    void OnEnable()
    {
        switchAction.Enable();
    }

    void OnDisable()
    {
        switchAction.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeBarImage = timeBar.GetComponent<Image>();
        time = maxTime;
        blueColor = timeBarImage.color;
        redColor = new Color(1f, 0f, 0f, 0.5f);

        foreach (GameObject obj in futureObjects)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in pastObjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = pastMat;
        }

        foreach (GameObject obj in persistentObjects)
        {
            obj.GetComponent<Renderer>().sharedMaterial = persistentMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (switchAction.WasPressedThisFrame() && pauseManager.paused == false && timeLocked == false)
        {
            isPast = !isPast;
            ApplyState();
        }

        TimeFrame();

        if (time <= 0)
            timeLocked = true;
        else if (time >= maxTime / 2f)
            timeLocked = false;

        if (timeLocked == true)
            timeBarImage.color = redColor;
        else
            timeBarImage.color = blueColor;

        Recharge();

        timeBarImage.fillAmount = time / maxTime;
    }

    void SetMat(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.sharedMaterial = isPast ? pastMat : futureMat;
    }

    void TimeFrame()
    {
        if (!isPast && time > 0)
        {
            time -= drainRate * Time.deltaTime;
            counter = 0;
        }

        if (time <= 0 && !isPast)
        {
            isPast = true;
            ApplyState();
        }
    }

    void Recharge()
    {
        if (time >= maxTime)
        {
            time = maxTime;
            return;
        }

        counter += Time.deltaTime;

        if (counter >= timeDelay)
        {
            time += rechargeRate * Time.deltaTime;
        }
    }

    void ApplyState()
    {
        if (isPast)
            audioSource.PlayOneShot(endSound, 0.2f);
        else
            audioSource.PlayOneShot(startSound, 0.2f);

        foreach (GameObject obj in pastObjects)
        {
            obj.SetActive(isPast);
            SetMat(obj);
        }

        foreach (GameObject obj in futureObjects)
        {
            obj.SetActive(!isPast);
            SetMat(obj);
        }

        // JADON
        SetPlayerMat();
    }

    // JADON
    void SetPlayerMat()
    {
        Renderer renderer = player.GetComponent<Renderer>();
        renderer.sharedMaterial = isPast ? playerPastMat : playerFutureMat;
    }
}

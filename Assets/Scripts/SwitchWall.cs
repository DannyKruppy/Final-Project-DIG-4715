using UnityEngine;

public class SwitchWallController : MonoBehaviour
{
    public GameObject[] switches;
    public Color offColor = Color.red;
    public Color onColor = Color.green;
    public Color lockedColor = Color.yellow;
    public bool[] switchStates;
    public bool allLocked = false;
    public GameObject invisibleWall;

    void Start()
    {
        switchStates = new bool[switches.Length];

        //reset switches in case unity is being weird
        for (int i = 0; i < switches.Length; i++)
        {
            SetSwitchColor(i, offColor);
            switchStates[i] = false;
        }

        Debug.Log("Total switches: " + switches.Length + " Active: 0");
    }
    public void ActivateSwitch(GameObject switchObj)
    {
        if (allLocked) return;

        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i] == switchObj)
            {
                switchStates[i] = !switchStates[i];
                SetSwitchColor(i, switchStates[i] ? onColor : offColor);
                break;
            }
        }

        CheckAllSwitches();
    }
    public void CheckAllSwitches()
    {
        int activeCount = 0;

        for (int i = 0; i < switchStates.Length; i++)
        {
            if (switchStates[i])
                activeCount++;
        }

        Debug.Log("Total switches: " + switches.Length + " | Active: " + activeCount);

        if (activeCount == switches.Length)
        {
            LockAllSwitches();
        }
    }
    public void LockAllSwitches()
    {
        allLocked = true;

        for (int i = 0; i < switches.Length; i++)
        {
            SetSwitchColor(i, lockedColor);
        }

        if (invisibleWall != null)
        {
            invisibleWall.SetActive(false);
            Debug.Log("All switches active. Invisible wall deactivated successfully.");
        }
        else
        {
            Debug.LogError("All switches active, but Invisible Wall reference is missing!");
        }
    }
    public void SetSwitchColor(int index, Color color)
    {
        Renderer rend = switches[index].GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}
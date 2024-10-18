using UnityEngine;

public class FlashManager : MonoBehaviour
{
    private int batteryCount; // Number of batteries collected

    void Start()
    {
        batteryCount = 0; // Initialize battery count
    }

    // Call this method when a battery is picked up
    public void AddBattery(int amount)
    {
        batteryCount += amount;
        Debug.Log("Battery picked up! Total batteries: " + batteryCount);
    }

    // Method to get the current battery count
    public int GetBatteryCount()
    {
        return batteryCount;
    }

    // Method to check if there are batteries available
    public bool HasBatteries()
    {
        return batteryCount > 0;
    }

    // Method to use a battery
    public void UseBattery()
    {
        if (HasBatteries())
        {
            batteryCount--;
            Debug.Log("Battery used! Remaining batteries: " + batteryCount);
        }
        else
        {
            Debug.Log("No batteries left to use!");
        }
    }
}
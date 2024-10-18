using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float batteryRechargeAmount = 100f;  // Amount to recharge the torch by
    public Flashlight torch;                        // Reference to the torch object

    // This method will be called when the battery is clicked
    private void OnMouseDown()
    {
        // Call the recharge method on the torch
        if (torch != null)
        {
            torch.ReplaceBattery(batteryRechargeAmount);
        }

        // Make the battery disappear
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("The battery that the player gains when they collect this.")]
    [SerializeField] int batteryWeight;
    [Tooltip("Key that needs to be pressed to collect this battery.")]
    [SerializeField] KeyCode CollectKey = KeyCode.E;

    [Header("References")]
    [Tooltip("The object that is shown when the player hovers.")]
    [SerializeField] GameObject[] HoverObject;
    [Tooltip("Reference to the FlashManager in the scene.")]
    [SerializeField] FlashManager flashlightManager; 

    private bool isCollected = false; // Flag to track if the battery has been collected
    private bool playerInRange = false; // Flag to track if player is in range
    private GameObject player; // Store reference to player

    private void Start()
    {
        if (flashlightManager == null)
        {
            Debug.LogError("FlashManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player") && !isCollected)
        {
            playerInRange = true; 
            player = other.gameObject;
            // Show pickup instructions or hover object
            foreach (GameObject obj in HoverObject) obj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the hover object when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            playerInRange = false; 
            player = null;
            foreach (GameObject obj in HoverObject) obj.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for battery collection input
        if (isCollected || !playerInRange) return; // If already collected, ignore further updates

        if (Input.GetKeyDown(CollectKey))
        {
            if (flashlightManager != null)
            {
                Debug.Log("Battery picked up");
                flashlightManager.AddBattery(batteryWeight);
                isCollected = true; // Set the flag to true to prevent double collection
                Destroy(this.gameObject); // Destroy this battery GameObject when collected
            }
            else
            {
                Debug.LogError("Cannot collect battery; FlashManager is not assigned.");
            }
        }
    }
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery2 : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("The battery that the player gains when they collect this.")][SerializeField] int batteryWeight;
    [Tooltip("Key that needs to be pressed to collect this battery.")][SerializeField] KeyCode CollectKey = KeyCode.E;

    [Header("References")]
    [Tooltip("The object that is shown when the player hovers.")][SerializeField] GameObject[] HoverObject;

    private FlashManager flashlightManager;
    private bool isCollected = false; // Flag to track if the battery has been collected

    private void Start()
    {
        flashlightManager = FindObjectOfType<FlashManager>();

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
            // Show pickup instructions or hover object
            foreach (GameObject obj in HoverObject) obj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the hover object when the player leaves the trigger zone
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in HoverObject) obj.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for battery collection input
        if (isCollected) return; // If already collected, ignore further updates

        if (Input.GetKeyDown(CollectKey))
        {
            if (flashlightManager != null)
            {
                Debug.Log("Battery picked up");
                flashlightManager.AddBattery(1);
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
*/
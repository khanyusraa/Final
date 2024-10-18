using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floppy1 : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public static int discsCount = 0;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the "E" key and is within range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickUpDisc();
        }
    }

    // Method to handle key pickup
    private void PickUpDisc()
    {
        discsCount++;
        Debug.Log("Disc " + discsCount + " picked up!");
        
        Destroy(gameObject);
    }

    // Detect when the player enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range to pick up the disc.");
        }
    }

    // Detect when the player leaves the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left the range of the disc.");
        }
    }
}

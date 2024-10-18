using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource terrainAudioSource;  // Assign the AudioSource directly in the Unity Inspector

    void Start()
    {
        // Ensure the AudioSource is assigned
        if (terrainAudioSource == null)
        {
            Debug.LogError("No AudioSource assigned to terrainAudioSource!");
        }
        else
        {
            terrainAudioSource.loop = true; // Set to loop if you want continuous play
        }
    }

    // Called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player") && terrainAudioSource != null)
        {
            terrainAudioSource.Play(); // Play the audio when the player enters the terrain
        }
    }

    // Called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Stop the audio when the player exits the terrain
        if (other.CompareTag("Player") && terrainAudioSource != null)
        {
            terrainAudioSource.Stop(); // Stop the audio when the player leaves the terrain
        }
    }
}

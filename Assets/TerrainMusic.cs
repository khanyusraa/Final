using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMusic : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip terrainAudio; // The audio clip for the terrain music
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Create an AudioSource component
        audioSource.clip = terrainAudio; // Assign the terrain audio clip
        audioSource.loop = true; // Set to loop if you want continuous play
    }

    // Called when another collider enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            audioSource.Play(); // Play the audio when the player enters the terrain
        }
    }

    // Called when another collider exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Stop the audio when the player exits the terrain
        if (other.CompareTag("Player"))
        {
            audioSource.Stop(); // Stop the audio when the player leaves the terrain
        }
    }
}

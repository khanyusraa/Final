using System.Collections;
using UnityEngine;

public class Walk : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] footstepSounds; // Array of footstep sounds
    [SerializeField] private float stepRate = 0.5f; // Time interval between steps
    [SerializeField] private float walkSpeedThreshold = 0.1f; // Speed threshold to trigger footsteps

    private AudioSource audioSource;
    private bool isPlaying = false; // Flag to track if sound is currently playing

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the player is moving based on input
        if (IsMoving())
        {
            // Check if the step sound should play and if not already playing
            if (!audioSource.isPlaying && !isPlaying)
            {
                PlayFootstepSound();
            }
        }
        else
        {
            // Reset the playing flag if not moving
            isPlaying = false;
        }
    }

    private bool IsMoving()
    {
        // Check for input from WASD keys
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }

    private void PlayFootstepSound()
    {
        // Play a random footstep sound from the array
        if (footstepSounds.Length > 0)
        {
            // Choose a random sound from the array
            int randomIndex = Random.Range(0, footstepSounds.Length);
            audioSource.clip = footstepSounds[randomIndex];

            // Set pitch to double if Shift is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                audioSource.pitch = 2.0f; // Speed up audio
            }
            else
            {
                audioSource.pitch = 1.0f; // Normal speed
            }

            audioSource.Play();
            isPlaying = true; // Set the flag to true while playing sound

            // Start a coroutine to wait before playing the next step
            StartCoroutine(StepDelay());
        }
    }

    private IEnumerator StepDelay()
    {
        // Wait for the step rate before allowing the next step sound
        yield return new WaitForSeconds(stepRate);
        isPlaying = false; // Reset the flag after the delay
    }
}
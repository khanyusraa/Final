using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public GameObject Lore;
    public GameObject[] loreElements;
    private bool isPlayerInRangeRobot = false;
    private FirstPersonController playerController;

    // Array to hold 4 audio clips
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        Lore.SetActive(false);
        playerController = FindObjectOfType<FirstPersonController>();

        // Get the AudioSource component attached to the robot
        audioSource = GetComponent<AudioSource>();

        // Optional: check if there are exactly 4 clips, else log a warning
        if (audioClips.Length != 4)
        {
            Debug.LogWarning("There should be exactly 4 audio clips assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRangeRobot && Input.GetKeyDown(KeyCode.R)) //check if they pressed Left Click near the robot
        {
            bool isActive = Lore.activeSelf;

            if (isActive)
            {
                CloseLoreMenu();
            }
            else
            {
                OpenLoreMenu();
            }

            // Toggle the lore's active state
            Lore.SetActive(!isActive);

            // Update lore elements visibility based on the floppyScript discs array
            UpdateLoreElements();

            // Play a random audio clip from the array
            PlayRandomAudioClip();
        }
    }
    private void OpenLoreMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerController.cameraCanMove = false;
        playerController.useSprintBar = false;
        playerController.crosshair = false;
        if (playerController.crosshairObject != null)
        {
            playerController.crosshairObject.gameObject.SetActive(false);  // Disable crosshair
        }

        if (playerController.sprintBarBG != null)
        {
            playerController.sprintBarBG.gameObject.SetActive(false);  // Disable sprint bar background
        }

        if (playerController.sprintBar != null)
        {
            playerController.sprintBar.gameObject.SetActive(false);  // Disable sprint bar
        }
    }

    private void CloseLoreMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.cameraCanMove = true;
        playerController.useSprintBar = true;
        playerController.crosshair = true;
        if (playerController.crosshairObject != null)
        {
            playerController.crosshairObject.gameObject.SetActive(true);  // Enable crosshair
        }

        if (playerController.sprintBarBG != null)
        {
            playerController.sprintBarBG.gameObject.SetActive(true);  // Enable sprint bar background
        }

        if (playerController.sprintBar != null)
        {
            playerController.sprintBar.gameObject.SetActive(true);  // Enable sprint bar
        }
    }
    private void UpdateLoreElements()
    {
        // Iterate over loreElements and activate/deactivate based on discsCount
        for (int i = 0; i < loreElements.Length; i++)
        {
            if (i < floppy1.discsCount) // Only activate lore elements up to the current discsCount
            {
                loreElements[i].SetActive(true);
            }
            else
            {
                loreElements[i].SetActive(false);
            }
        }
    }

    private void PlayRandomAudioClip()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length); // Get a random index between 0 and the number of clips
            AudioClip clipToPlay = audioClips[randomIndex];

            // Check if the AudioSource or AudioClip is null
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is null. Make sure the AudioSource component is attached to the robot.");
            }
            else if (clipToPlay == null)
            {
                Debug.LogError("Selected AudioClip is null. Check the audioClips[] array for null entries.");
            }
            else
            {
                audioSource.Stop(); // Stop any currently playing audio
                audioSource.PlayOneShot(clipToPlay); // Play the new clip
            }
        }
        else
        {
            Debug.LogWarning("No audio clips assigned to the array.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRangeRobot = true;
            Debug.Log("Player entered the range of the robot.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRangeRobot = false;
            Debug.Log("Player left the range of the robot.");
        }
    }
}

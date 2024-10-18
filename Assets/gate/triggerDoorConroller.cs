using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDoorConroller : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private AudioSource doorAudioSource = null; // Reference to the AudioSource
    [SerializeField] private AudioClip openSound = null; // Audio clip for opening the door
    [SerializeField] private AudioClip closeSound = null; // Audio clip for closing the door

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    public GameObject cube2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("open2", 0, 0.0f);
                gameObject.SetActive(false);
                
                // Play the open sound
                if (doorAudioSource != null && openSound != null)
                {
                    doorAudioSource.PlayOneShot(openSound);
                }

                Debug.Log("Opening gate");
            }
            else if (closeTrigger)
            {
                myDoor.Play("close2", 0, 0.0f);
                gameObject.SetActive(false);

                // Play the close sound
                if (doorAudioSource != null && closeSound != null)
                {
                    doorAudioSource.PlayOneShot(closeSound);
                }

                Debug.Log("Closing gate");
                cube2.SetActive(false);
            }
        }
    }
}

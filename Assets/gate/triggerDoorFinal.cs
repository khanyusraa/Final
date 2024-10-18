using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class triggerDoorFinal : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private AudioClip openSound = null; // Audio clip for opening the door
    [SerializeField] private AudioClip closeSound = null; // Audio clip for closing the door

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                myDoor.Play("open1", 0, 0.0f);
                AudioSource.PlayClipAtPoint(openSound, transform.position);
                gameObject.SetActive(false);

                // Play the open sound at the door's position
                if (openSound != null)
                {
                   
                    Debug.Log("Opening sound played.");
                }
                else
                {
                    Debug.LogWarning("Open sound is not assigned.");
                }

                Debug.Log("Opening gate");
            }
            else if (closeTrigger)
            {
                myDoor.Play("close1", 0, 0.0f);
                gameObject.SetActive(false);

                // Play the close sound at the door's position
                if (closeSound != null)
                {
                    AudioSource.PlayClipAtPoint(closeSound, transform.position);
                    Debug.Log("Closing sound played.");
                }
                else
                {
                    Debug.LogWarning("Close sound is not assigned.");
                }

                if (key.isUnlocked)
                {
                    SceneManager.LoadScene(0);
                }

                Debug.Log("Closing gate");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class key : MonoBehaviour
{
    private bool isPlayerInRange = false;  // Check if the player is in range to interact
    public Animator GateAnimationController;          // Reference to the gate's Animator
    public keyTrigger trigger1;
    public keyTrigger2 trigger2;
    public endCube end;
    [SerializeField] private TextMeshProUGUI gateTextTMP; // Reference to TextMeshPro component
    public static bool isUnlocked;

    private void Start()
    {
        GateAnimationController = GetComponent<Animator>();
        gateTextTMP.enabled = false;
        isUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the "E" key and is within range
        if (isPlayerInRange &&
            Input.GetKeyDown(KeyCode.E))
        {
            PickUpKey();
        }
    }

    // Method to handle key pickup
    private void PickUpKey()
    {
        Debug.Log("Key picked up!");
        isUnlocked = true;

        if (trigger1 != null && trigger2 != null)
        {
            trigger1.keyTrigger1 = true;
            trigger2.keyTriggerr2 = true;
            end.endTrigger = true;
            Debug.Log("Triggers updated: trigger1 = " + trigger1.keyTrigger1 + ", trigger2 = " + trigger2.keyTriggerr2);
        }
        else
        {
            Debug.LogError("Triggers are not assigned!");
        }

        // Make the key disappear
        Destroy(gameObject);

        gateTextTMP.enabled = true;
        StartCoroutine(HideTextAfterDelay(1f));
    }
    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        gateTextTMP.enabled = false;  // Disable the text
    }

    // This function will be used to open the gate
    private void OpenGate()
    {
        if (GateAnimationController != null)
        {
            GateAnimationController.SetBool("isOpen", true);
            Debug.Log("Gate opening...");
        }
        else
        {
            Debug.LogError("Gate Animator is not assigned!");
        }

       
    }

    // Detect when the player enters the trigger collider of the key
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;  
            Debug.Log("Player in range to pick up the key.");
        }
    }

    // Detect when the player leaves the trigger collider of the key
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;  
            Debug.Log("Player left the range of the key.");
        }
    }
}

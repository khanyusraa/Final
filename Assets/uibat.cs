using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class uibat : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight; // Reference to the Flashlight class
    [SerializeField] private TextMeshProUGUI batteryTextTMP; // Reference to TextMeshPro component

    void Start()
    {
        batteryTextTMP.enabled = false;
        if (flashlight == null)
        {
            flashlight = FindObjectOfType<Flashlight>(); // Automatically find the Flashlight component in the scene
        }

        if (batteryTextTMP == null)
        {
            Debug.LogError("TextMeshProUGUI is not assigned in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUp.PickedUp)
        {
            batteryTextTMP.enabled = true;
        }
        if (flashlight != null && batteryTextTMP != null)
        {
            // Access the chargePer variable from the Flashlight class and update the TextMeshProUGUI text
            batteryTextTMP.text = $"Battery: {flashlight.chargePer:F1}";
        }
    }
}


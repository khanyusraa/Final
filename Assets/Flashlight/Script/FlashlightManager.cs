/*using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum FlashLightState
{
    Off, // When the flashlight is off
    On, // When the flashlight is on
    Dead // When there is no battery left
}

[RequireComponent(typeof(AudioSource))]
public class FlashlightManager : MonoBehaviour
{
    [Header("Options")]
    [Tooltip("The speed that the battery is lost at")][Range(0.0f, 2f)][SerializeField] float batteryLossTick = 0.5f;

    [Tooltip("This is the amount of battery that the player starts with.")][SerializeField] int startBattery = 100;

    [Tooltip("This is the amount of battery that the player currently has.")] public int currentBattery;

    [Tooltip("The current state of the flashlight.")] public FlashLightState state;

    [Tooltip("Is the flashlight on?")] private bool flashlightIsOn;

    [Tooltip("The key is required to be pressed to turn on/off the flashlight.")][SerializeField] KeyCode ToggleKey = KeyCode.F;

    [Header("Referernces")]

    [Tooltip("The light that will be shown if the flashlight is on.")][SerializeField] GameObject FlashlightLight;

    [Tooltip("Sounds that will be played when the flashlight is toggled.")][SerializeField] AudioClip FlashlightOn_FX, FlashlightOff_FX;



    private void Start()
    {
        currentBattery = startBattery; // Set the current battery to the start battery when the game starts

        InvokeRepeating(nameof(LoseBattery), 0, batteryLossTick); // Loses the battery at a set interval point
    }

    private void Update()
    {
        if (Input.GetKeyDown(ToggleKey)) ToggleFlashLight(); // Toggles the flashlight

        // Handles the light that will be shown
        if (state == FlashLightState.Off) FlashlightLight.SetActive(false);
        else if (state == FlashLightState.Dead) FlashlightLight.SetActive(false);
        else if (state == FlashLightState.On) FlashlightLight.SetActive(true);

        //Handles the battery being dead
        if (currentBattery <= 0)
        {
            currentBattery = 0;
            state = FlashLightState.Dead;
            flashlightIsOn = false; // Automatically turns the flashlight off
        }
    }
    public void GainBattery(int amount) // Handle the gaining of battery
    {
        if (currentBattery == 0)
        {
            state = FlashLightState.On;
            flashlightIsOn = true;
        }

        if (currentBattery + amount > startBattery)
            currentBattery = startBattery; // Automatically cause the battery to be the maximum
        else
            currentBattery += amount; // Adds the amount of battery to the current battery
    }

    private void LoseBattery() // Handle the lose of battery
    {
        if (state == FlashLightState.On) currentBattery--; // Subtracts the battery by 1 if the flashlight is on
    }

    private void ToggleFlashLight() // Toggle the on/off state of the flashlight
    {
        flashlightIsOn = !flashlightIsOn;

        if (state == FlashLightState.Dead) flashlightIsOn = false; // Automatically overrides the state if theres no battery


        // Handles the light/sound that are seen or heard when the player toggles the flashlight
        if (flashlightIsOn)
        {
            if (FlashlightOn_FX != null) GetComponent<AudioSource>().PlayOneShot(FlashlightOn_FX); // Plays flashlight on sound
            state = FlashLightState.On; // Turns the flashlight off
        }
        else
        {
            if (FlashlightOn_FX != null) GetComponent<AudioSource>().PlayOneShot(FlashlightOff_FX); // Play flashlight off sound
            state = FlashLightState.Off; // Turns flashlight off
        }
    }
} */
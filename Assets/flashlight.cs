using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] FlashManager FlashManager;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip flashlightSound; // The audio clip for the flashlight sound
    private AudioSource audioSource;

    public bool drainOverTime;
    public float maxBrightness;
    public float minBrightness;
    public float drainRate;
    public float range = 50f; // Range of the flashlight
    public LayerMask monsterLayer; // The layer for the monster
    public bool charged;
    public float chargePer;

    public Light m_light;
    private monsterAI currentMonster = null; // Keep track of the current monster being hit
    private bool hasBeenPickedUp = false;  // Flag to check if flashlight has been picked up

    void Start()
    {
        m_light = GetComponent<Light>();
        charged = true;
        m_light.enabled = false;
        m_light.intensity = maxBrightness; // Start at full brightness

        audioSource = gameObject.AddComponent<AudioSource>(); // Create a new AudioSource for flashlight sound
        audioSource.clip = flashlightSound; // Assign the flashlight sound clip
        audioSource.playOnAwake = false; // Prevent sound from playing on awake
    }

    void Update()
    {
        // If flashlight hasn't been picked up, prevent toggling it
        if (!hasBeenPickedUp) return;

        m_light.intensity = Mathf.Clamp(m_light.intensity, minBrightness, maxBrightness);

        // Drain battery over time if the flashlight is enabled
        if (drainOverTime && m_light.enabled)
        {
            if (m_light.intensity > minBrightness)
            {
                m_light.intensity -= Time.deltaTime * (drainRate / 1000);
                UpdateBatteryPercentage();
            }
        }

        // Check if the battery is dead
        if (m_light.intensity <= minBrightness)
        {
            m_light.enabled = false; // Turn off the flashlight
            Debug.Log("Battery died");
            charged = false;

            // If the flashlight battery dies, resume the monster's movement if it was stopped
            if (currentMonster != null)
            {
                currentMonster.StopChasing(false); // Resume movement
                currentMonster = null; // Reset current monster
            }
        }

        // Toggle light on/off with F key
        if (charged)
        {
            if (Input.GetKeyDown(KeyCode.F) && !audioSource.isPlaying)
            {
                m_light.enabled = !m_light.enabled;
                audioSource.Play(); // Play the flashlight sound

                // If light is turned off, ensure the monster resumes movement
                if (!m_light.enabled && currentMonster != null)
                {
                    currentMonster.StopChasing(false); // Resume movement
                    currentMonster = null; // Reset current monster
                }
            }
        }

        // Replace battery with R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Use a battery from the FlashlightManager
            if (FlashManager.HasBatteries())
            {
                FlashManager.UseBattery(); // Use a battery
                ReplaceBattery(3f); // Add 1f to the light's intensity when replacing the battery
            }
            else
            {
                Debug.Log("No batteries available to replace!");
            }
        }

        // Raycast to detect if the light is hitting the monster
        if (m_light.enabled)
        {
            Ray ray = new Ray(m_light.transform.position, m_light.transform.forward);
            RaycastHit hit;

            // Check if the ray hits something within the range and if it's on the monster layer
            if (Physics.Raycast(ray, out hit, range, monsterLayer))
            {
                monsterAI monster = hit.collider.GetComponent<monsterAI>(); // Assuming your AI script is "MonsterAI"
                if (monster != null)
                {
                    if (currentMonster == null)
                    {
                        Debug.Log("Flashlight hit the monster! Reducing speed...");
                        monster.StopChasing(true);  // Stop monster when flashlight hits
                    }
                    currentMonster = monster; // Track the monster being hit
                }
            }
            else if (currentMonster != null)
            {
                Debug.Log("Flashlight no longer hitting the monster. Resuming movement...");
                currentMonster.StopChasing(false); // Resume movement when flashlight no longer hits
                currentMonster = null; // Reset current monster
            }
        }
    }

    public void ReplaceBattery(float amount)
    {
        m_light.intensity += amount;
        charged = true; // Make sure flashlight is charged again
        m_light.enabled = true; // Turn the light on if it was off
        UpdateBatteryPercentage();
    }

    // Call this method from the pickUp script when the player picks up the flashlight
    public void PickUpFlashlight()
    {
        hasBeenPickedUp = true;
        Debug.Log("Flashlight has been picked up and is now usable.");
    }

    private void UpdateBatteryPercentage()
    {
        chargePer = (m_light.intensity - minBrightness) / (maxBrightness - minBrightness) * 100f;
        Debug.Log($"Battery percentage: {chargePer}%");
    }


}
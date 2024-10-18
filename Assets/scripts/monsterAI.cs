using UnityEngine;
using UnityEngine.AI;

public class monsterAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navAgent;
    private Animator animator;

    public float attackDistance = 1f;
    public bool attack = false;
    public bool stop = false;

    public float runSpeed = 8.0f;
    public float walkSpeed = 3.0f;
    public float slowedSpeed = 0f;

    public float detectionRange = 30f;

    private float attackDuration = 1f;
    private float attackEndTime = 0f;
    private float growlCooldown = 10f;   // Cooldown time for growl
    private float nextGrowlTime = 0f;    // Time when the next growl can occur

    public AttackSupport pulseEffect;

    public AudioSource attackAudioSource;   // AudioSource for attack
    public AudioSource growl1AudioSource;   // AudioSource for growl1
    public AudioSource growl2AudioSource;   // AudioSource for growl2
    public AudioSource growl3AudioSource;   // AudioSource for growl3
    public AudioSource chaseAudioSource;    // AudioSource for chase sound

    private Flashlight flashlight;   // Reference to the Flashlight class

    // Monster spawn points
    public Vector3[] monsterPositions = new Vector3[]
    {
        new Vector3(1000f, 1.1f, -450),
        new Vector3(-14.76f, 1.1f, -409.38f),
        new Vector3(-100f, 1.1f, 450f),
        new Vector3(-1000f, 1.1f, -3000f),
        new Vector3(-100f, 1.1f, -300f),
        new Vector3(15f, 1.1f, -409.38f),
        new Vector3(-14.76f, 1.1f, -409.38f)
    };

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        pulseEffect = FindObjectOfType<AttackSupport>();

        flashlight = FindObjectOfType<Flashlight>();  // Find the Flashlight script in the scene

        navAgent.speed = walkSpeed;
        transform.position = new Vector3(-1000f, 1.1f, -3000f);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Handle attacking behavior when within attack range
            if (distanceToPlayer <= attackDistance && !stop)
            {
                if (!attack)
                {
                    navAgent.isStopped = true; // Stop the monster from moving
                    attack = true;

                    if (animator != null)
                    {
                        animator.SetBool("attack", true);  // Trigger attack animation
                    }

                    // Play attack sound only once when attack starts
                    if (attackAudioSource != null && !attackAudioSource.isPlaying)
                    {
                        attackAudioSource.Play(); // Play attack sound
                    }

                    attackEndTime = Time.time + attackDuration; // Set time to end attack
                }
                pulseEffect.pulseMaterial.SetFloat("_PulseSpeed", 2.0f);  // Speed up the pulse
                pulseEffect.pulseMaterial.SetFloat("_BorderThickness", 0.1f);  // Make border thicker
                Debug.Log("Made border thicker");
            }
            else if (attack)
            {
                // Finish attack after attackDuration has passed
                if (Time.time >= attackEndTime)
                {
                    player.position = new Vector3(-4.43f, 1.75f, 4.51f); // Reset player position (death)

                    

                    int randomIndex = Random.Range(0, 7);  // Generates a number between 0 and 6

                    transform.position = monsterPositions[randomIndex]; // Reset monster position

                    navAgent.isStopped = false; // Resume movement after attack
                    attack = false;

                    if (animator != null)
                    {
                        animator.SetBool("attack", false);  // Reset attack animation
                    }

                    pulseEffect.pulseMaterial.SetFloat("_PulseSpeed", 0f);  // End pulse
                    pulseEffect.pulseMaterial.SetFloat("_BorderThickness", 0f);  // Make border invisible
                    Debug.Log("Reset pulse");
                    flashlight.m_light.intensity = flashlight.maxBrightness;
                }
            }

            // Handle growling every 10 seconds if chasing
            if (IsChasingPlayer(distanceToPlayer))
            {
                // Random growl sound every 10 seconds
                if (Time.time >= nextGrowlTime)
                {
                    PlayRandomGrowl();
                    nextGrowlTime = Time.time + growlCooldown;
                }

                // Play chase audio only if the flashlight battery is dead (charged == false)
                if (distanceToPlayer <= detectionRange && flashlight != null && !flashlight.charged)
                {
                    PlayChaseAudio();
                }
            }

            // Handle movement behavior (run or walk based on distance), only if not attacking
            if (!attack && !stop)  // Only move if not attacking or hit by the flashlight
            {
                if (distanceToPlayer <= detectionRange)
                {
                    // Monster "sees" the player, start running
                    navAgent.speed = runSpeed;
                    navAgent.SetDestination(player.position);  // Move toward the player
                }
                else
                {
                    // Monster can't "see" the player, walk at normal speed
                    navAgent.speed = walkSpeed;
                    navAgent.SetDestination(player.position);  // Move toward the player
                }
            }
        }

        // Update the 'stop' boolean in the animator to trigger stopping animation
        if (animator != null)
        {
            animator.SetBool("stop", stop);
        }
    }

    // Method to check if the monster is chasing the player
    private bool IsChasingPlayer(float distanceToPlayer)
    {
        return distanceToPlayer <= detectionRange && !attack && !stop;
    }

    // Method to play a random growl sound from the respective AudioSource
    private void PlayRandomGrowl()
    {
        int randomGrowl = Random.Range(1, 4);  // Randomly pick 1, 2, or 3

        switch (randomGrowl)
        {
            case 1:
                if (!growl1AudioSource.isPlaying)
                {
                    growl1AudioSource.Play();
                }
                break;
            case 2:
                if (!growl2AudioSource.isPlaying)
                {
                    growl2AudioSource.Play();
                }
                break;
            case 3:
                if (!growl3AudioSource.isPlaying)
                {
                    growl3AudioSource.Play();
                }
                break;
        }
    }

    // Method to play chase audio
    private void PlayChaseAudio()
    {
        if (!chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Play();
            pulseEffect.pulseMaterial.SetFloat("_PulseSpeed", 2.0f);  // Speed up the pulse
            pulseEffect.pulseMaterial.SetFloat("_BorderThickness", 0.02f);  // Make border thicker
            Debug.Log("Rest pulse");
        }
    }

    // Method to reduce or restore the monster's speed when hit by the flashlight
    public void StopChasing(bool stopChasing)
    {
        stop = stopChasing; // Update stop bool to control animation

        if (stopChasing)
        {
            navAgent.speed = slowedSpeed;  // Set speed to 0 when hit by the flashlight
            navAgent.isStopped = true;     // Completely stop movement
        }
        else
        {
            navAgent.isStopped = false;    // Resume movement
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            navAgent.speed = distanceToPlayer <= detectionRange ? runSpeed : walkSpeed;
        }
    }
}

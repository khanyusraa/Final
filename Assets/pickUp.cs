using UnityEngine;

public class pickUp : MonoBehaviour
{
    [SerializeField] GameObject[] HoverObject;

    private bool isCollected = false; // Flag to track if the battery has been collected
    private bool playerInRange = false; // Flag to track if player is in range
    private GameObject player; // Store reference to player
    private Flashlight flashlight; // Dynamically find the flashlight component

    public static bool PickedUp = false;

    private void Start()
    {
        // Dynamically find the flashlight in the scene
        flashlight = GameObject.FindObjectOfType<Flashlight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            playerInRange = true;
            player = other.gameObject;
            foreach (GameObject obj in HoverObject) obj.SetActive(true);
            Debug.Log("Player entered the pickup range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            foreach (GameObject obj in HoverObject) obj.SetActive(false);
            Debug.Log("Player exited the pickup range");
        }
    }

    void Update()
    {
        if (isCollected || !playerInRange) return;

        if (Input.GetKeyUp(KeyCode.E))
        {
            PickUpTorch();
            isCollected = true;
            PickedUp = true;
        };
    }

    private void PickUpTorch()
    {
        if (flashlight != null)
        {
            flashlight.PickUpFlashlight(); // Notify the Flashlight script that it's been picked up
            Debug.Log("You picked up the torch");
        }
        else
        {
            Debug.LogError("Flashlight not found in the scene!");
        }

        Destroy(gameObject);
    }
}

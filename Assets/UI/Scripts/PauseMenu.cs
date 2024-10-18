using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused = false;

    private FirstPersonController playerController;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        playerController = FindObjectOfType<FirstPersonController>();
        if (playerController == null )
        {
            Debug.Log("Null player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex==1)
        {
            if (isPaused && SceneManager.GetActiveScene().buildIndex==1)
            {
                ResumeGame();
            }
            else if (!isPaused && SceneManager.GetActiveScene().buildIndex == 1)
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
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
        Debug.Log("player locked");
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
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
        Debug.Log("player enabled");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private FirstPersonController playerController;
    private GameObject batteryUI; 

    void Start()
    {
        Debug.Log("Main Menu Start");
        //playerController = FindObjectOfType<FirstPersonController>();
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        //playerController.cameraCanMove = false;
        //playerController.useSprintBar = false;
        //playerController.crosshair = false;
        //if (playerController.crosshairObject != null)
        //{
        //    playerController.crosshairObject.gameObject.SetActive(false);  // Disable crosshair
        //}

        //if (playerController.sprintBarBG != null)
        //{
        //    playerController.sprintBarBG.gameObject.SetActive(false);  // Disable sprint bar background
        //}

        //if (playerController.sprintBar != null)
        //{
        //    playerController.sprintBar.gameObject.SetActive(false);  // Disable sprint bar
        //}
    }

    public void PlayGame()
    {
        Debug.Log("Main Menu Play");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = true;
        //playerController.cameraCanMove = true;
        //playerController.useSprintBar = true;
        //playerController.crosshair = true;
        //if (playerController.crosshairObject != null)
        //{
        //    playerController.crosshairObject.gameObject.SetActive(true);  // Enable crosshair
        //}

        //if (playerController.sprintBarBG != null)
        //{
        //    playerController.sprintBarBG.gameObject.SetActive(true);  // Enable sprint bar background
        //}

        //if (playerController.sprintBar != null)
        //{
        //    playerController.sprintBar.gameObject.SetActive(true);  // Enable sprint bar
        //}
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
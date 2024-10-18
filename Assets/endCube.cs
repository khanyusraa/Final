using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endCube : MonoBehaviour
{
    [SerializeField] public bool endTrigger = false;

    void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {

            if (endTrigger)
            {
                QuitGame();
                gameObject.SetActive(false);
                
            }
        }
    }
}

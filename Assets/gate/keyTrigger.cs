using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyTrigger : MonoBehaviour
{



    [SerializeField] private Animator myDoor = null;

   
    [SerializeField] public bool keyTrigger1 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
             if (keyTrigger1)
            {
                myDoor.Play("open1", 0, 0.0f);
                gameObject.SetActive(false);
                Debug.Log("key gate");
            }
        }
    }
}
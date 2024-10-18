using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyTrigger2 : MonoBehaviour
{

    [SerializeField] private Animator myDoor = null;


    [SerializeField] public bool keyTriggerr2 = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (keyTriggerr2)
            {
                myDoor.Play("open2", 0, 0.0f);
                gameObject.SetActive(false);
                Debug.Log("key gate");
            }
        }
    }
}

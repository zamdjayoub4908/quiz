using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelDetect : MonoBehaviour
{  
    public GameObject panel;
    // Reference to the UI Button (assign this in the Inspector)
    public Button playButton;
    bool inside = false;
    bool quizfinish = false;
    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the door GameObject



        // Check if the openButton is assigned and disable it at the start
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
            playButton.onClick.AddListener(play); // Add click listener to the button
        }
        else
        {
            Debug.LogError("Open Button is not assigned. Please assign the button in the Inspector.");
        }
    }

    // This method is called when another collider enters the trigger collider attached to the door
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the tag "Player"
        if (other.CompareTag("Player"))
        {
            if (!quizfinish) { 
            inside = true;

            playButton.gameObject.SetActive(true);
            }

        }
    }

    // This method is called when another collider exits the trigger collider attached to the door
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider has the tag "Player"
        if (other.CompareTag("Player"))
        {
            inside = false;
            playButton.gameObject.SetActive(false);

        }
    }

    // Method to open the door
    private void play()
    {
        if (inside)
        {

            playButton.gameObject.SetActive(false);
            panel.SetActive(true);
            quizfinish = true;
        }
    }
}


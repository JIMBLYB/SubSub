using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script handling if the submarine should be 
/// sinking or not
/// </summary>
public class HealthControllerNoArduino : MonoBehaviour
{
    // Starts at 101 to allow the UI to update correctly,
    // by lowering the health by 1 immediately
    [SerializeField]
    public int health = 101; 
    [SerializeField]
    private int healthLossRate; // How much health should be lost at once

    [Header("Debugging")]
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private GameObject deathPanel;

    // Sinking event, only subscribed by HealthController.cs' Sink() method
    public delegate void Submarine();
    public static event Submarine Sinking;

    private void Start()
    {
        Sink(); // Calling this method in Start calibrates the UI
        // Subscribes Sink() to the Sinking method
        // Called by any of the 3 Valve.cs scripts
        Sinking += Sink;
    }

    private void Update()
    {
        // Resets the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            deathPanel.SetActive(false);
            health = 100;
        }
    }
    
    /// <summary>
    /// Handles the loss of health
    /// </summary>
    public void Sink()
    {
        health -= healthLossRate;
        healthText.text = "Remaining Health: " + health;

        // Ends the game on 0 health
        if (health <= 0)
        {
            Sunken();
        }
    }

    /// <summary>
    /// Handles anything related to the end of the game
    /// </summary>
    private void Sunken()
    {
        deathPanel.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script handling if the submarine should be 
/// sinking or not
/// </summary>
public class HealthController : MonoBehaviour
{
    // Starts at 101 to allow the UI to update correctly,
    // by lowering the health by 1 immediately
    public static int health = 101; 
    [SerializeField]
    private int healthLossRate; // How much health should be lost at once

    public Arduino arduino;

    [Header("Debugging")]
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private GameObject deathPanel;

    private void Start()
    {
        Sink(); // Calling this method in Start calibrates the UI
        // Subscribes Sink() to the Sinking method
        // Called by any of the 3 Valve.cs scripts
        Valve.Sinking += Sink;
    }

    private void Update()
    {
        // Resets the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            deathPanel.SetActive(false);
            health = 100;
            arduino.WriteToArduino("r"); // Tells the arduino to reset the motor
        }
    }
    
    /// <summary>
    /// Handles the loss of health
    /// </summary>
    private void Sink()
    {
        health -= healthLossRate;
        arduino.WriteToArduino("s"); // Tells the arduino to rotate the motor

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

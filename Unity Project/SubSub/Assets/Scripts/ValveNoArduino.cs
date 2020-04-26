using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class handling all of the inputs and outputs of a valve
/// </summary>
public class ValveNoArduino : MonoBehaviour
{
    [SerializeField]
    public float currentTurnAmount; // Set in Arduino.cs
    [SerializeField]
    private float turnSensitivity; // How much of an angle one notch on the rotary encoder should translate to
    [SerializeField]
    private float turnLenciency;  // The tolerance for the correct angle (Game is too difficult if you need the perfect amount)

    private float desiredAngle; // Angle needed to prevent sinking (The point that turnLeniency adds to)
    private bool shouldSink; // If this valve is causing the submarine to sink

    [SerializeField]
    private Text turnText;
    [SerializeField]
    private Text desiredTurnText;

    public HealthControllerNoArduino healthController;


    [Header("Keyboard Controls")]
    [SerializeField]
    private KeyCode turnLeft;
    [SerializeField]
    private KeyCode turnRight;

    private void Start()
    {
        // Resets turn amount at the start of the game
        // Since the value is set by the arduino, it can change before the the game is started
        currentTurnAmount = 0; 

        StartCoroutine(AngleDecider());
    }

    /// <summary>
    /// A 1/10 chance of changing the desired angle each second
    /// </summary>
    /// <returns> A 1 second timer</returns>
    private IEnumerator AngleDecider()
    {
        if (Random.value >= .9f) // 1/10 chance
        {
            desiredAngle = Random.Range(0, 360);
            desiredTurnText.text = "Desired Angle: " + desiredAngle;
        }

        // Stops overrotation putting you a full rotation or more past the desired angle
        currentTurnAmount = currentTurnAmount % 360;

        // Updates the UI
        turnText.text = currentTurnAmount.ToString();

        // Checks if the submarine should sink and invokes the sinking method
        if (shouldSink)
        {
            healthController.Sink();
        }

        // A one second wait before restarting the coroutine for a loop
        yield return new WaitForSeconds(1);
        StartCoroutine(AngleDecider());
    }

    private void Update()
    {
        // Changes turn amount based on keyboard inputs
        if (Input.GetKey(turnLeft) && !Input.GetKey(turnRight))
        {
            currentTurnAmount += turnSensitivity;
        }
        else if (Input.GetKey(turnRight) && !Input.GetKey(turnLeft))
        {
            currentTurnAmount -= turnSensitivity;
        }

        // Checks if the angle is in the correct range and therefore if the submarine should sink or not
        if (currentTurnAmount >= desiredAngle - turnLenciency
         && currentTurnAmount <= desiredAngle + turnLenciency)
        {
            shouldSink = false;
        }
        else
        {
            shouldSink = true;
        }

        // Rotates the dial to the correct position
        transform.eulerAngles = new Vector3(0, 0, -currentTurnAmount);
    }
}

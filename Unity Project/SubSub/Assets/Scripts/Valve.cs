using System.Collections;
using UnityEngine;

/// <summary>
/// Class handling all of the inputs and outputs of a valve
/// </summary>
public class Valve : MonoBehaviour
{
    [SerializeField]
    public float currentTurnAmount; // Set in Arduino.cs
    [SerializeField]
    private float turnSensitivity; // How much of an angle one notch on the rotary encoder should translate to
    [SerializeField]
    private float turnLenciency;  // The tolerance for the correct angle (Game is too difficult if you need the perfect amount)

    private float desiredAngle; // Angle needed to prevent sinking (The point that turnLeniency adds to)
    private bool shouldSink; // If this valve is causing the submarine to sink

    // Sinking event, only subscribed by HealthController.cs' Sink() method
    public delegate void Submarine();
    public static event Submarine Sinking;

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
        }

        // Checks if the submarine should sink and invokes the event subscribed by HealthController
        // Putting it in this coroutine ensures that it will only check once per second,
        // and using an event means that it will only work once per second even if the 
        // other valves also run it
        if (shouldSink)
        {
            Sinking();
        }

        // A one second wait before restarting the coroutine for a loop
        yield return new WaitForSeconds(1);
        StartCoroutine(AngleDecider());
    }

    private void Update()
    {
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
    }
}

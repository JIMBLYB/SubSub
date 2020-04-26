using System.IO.Ports;
using System;
using UnityEngine;

/// <summary>
/// Class handling all communication with the Arduino
/// </summary>
public class Arduino : MonoBehaviour
{
    // Sets port of arduino
    // since I'm using TinkerCAD for my circuit,
    // this isn't actually neccessary
    private int arduino = 1;
    private SerialPort serial = null;

    private string arduinoOutput;


    // The rotary encoders
    // Update() sends the turn value to the valves
    // through these variables
    public Valve rotaryEncoder1;
    public Valve rotaryEncoder2;
    public Valve rotaryEncoder3;

    void Start()
    {
        Debug.Log("Attempting Serial: " + arduino);

        // Sets up serial communication between Unity
        // and the Arduino
        serial = new SerialPort("\\\\.\\COM" + arduino, 9600);
        serial.ReadTimeout = 50;
        serial.Open();

        Debug.Log("Connected");
    }

    void Update()
    {
        arduinoOutput = ReadFromArduino(50); // Reads the arduino's output
        // Sends the encoder values to the correct instances of the Valve.cs script
        rotaryEncoder1.currentTurnAmount = int.Parse(arduinoOutput.Split()[0]);
        rotaryEncoder2.currentTurnAmount = int.Parse(arduinoOutput.Split()[2]);
        rotaryEncoder3.currentTurnAmount = int.Parse(arduinoOutput.Split()[4]);
    }

    /// <summary>
    /// Sends a string over the serial
    /// connection to the arduino
    /// </summary>
    /// <param name="message">The message to write</param>
    public void WriteToArduino(string message)
    {
        serial.WriteLine(message);
        // Sometimes the port will wait for
        // a certain amount of data to be written
        // before sending it, this forces it to
        // write whatever it currently has
        serial.BaseStream.Flush();
    }

    /// <summary>
    /// Reads the string sent by the arduino
    /// </summary>
    /// <param name="timeout">Milliseconds to wait before a timeout exception</param>
    /// <returns>Serial line written by arduino</returns>
    public string ReadFromArduino(int timeout = 0)
    {
        serial.ReadTimeout = timeout;
        try
        {
            return serial.ReadLine();
        }
        catch (TimeoutException e)
        {
            return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    [SerializeField]
    private float currentTurnAmount;
    [SerializeField]
    private float turnSensitivity;
    private float turnLenciency;

    private float desiredAngle;
    private bool shouldSink;

    public HealthControl healthLeft;

    [Header ("Keyboard Debug")]
    [SerializeField]
    private KeyCode leftTurn;
    [SerializeField]
    private KeyCode rightTurn;

    private void Start()
    {
        currentTurnAmount = 0;
        StartCoroutine(AngleDecider());
    }

    private IEnumerator AngleDecider()
    {
        if (Random.value >= .75f)
        {
            desiredAngle = Random.Range(0, 360);
        }
        print(desiredAngle);
        Sink();
        yield return new WaitForSeconds(1);
        StartCoroutine(AngleDecider());
    }

    private void Update()
    {
        if (Input.GetKey(leftTurn) && !Input.GetKeyDown(rightTurn))
        {
            currentTurnAmount -= turnSensitivity;
        }
        else if(Input.GetKey(rightTurn) && !Input.GetKeyDown(leftTurn))
        {
            currentTurnAmount += turnSensitivity;
        }

        if (currentTurnAmount >= desiredAngle - turnLenciency && currentTurnAmount <= desiredAngle + turnLenciency)
        {
            shouldSink = false;
        }
        else
        {
            shouldSink = true;
        }
    }

    private void Sink()
    {
        if (shouldSink)
        {
            shouldSink = false;
        }
    }
}

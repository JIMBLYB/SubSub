using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    [SerializeField]
    private float currentTurnAmount;
    [SerializeField]
    private float turnSensitivity;
    [SerializeField]
    private float turnLenciency;

    private float desiredAngle;
    private bool shouldSink;

    public delegate void Submarine();
    public static event Submarine Sinking;

    [Header ("Keyboard Debug")]
    [SerializeField]
    private KeyCode leftTurn;
    [SerializeField]
    private KeyCode rightTurn;
    [SerializeField]
    private Text positionText;

    private void Start()
    {
        currentTurnAmount = 0;
        StartCoroutine(AngleDecider());
    }

    private IEnumerator AngleDecider()
    {
        if (Random.value >= .9f)
        {
            desiredAngle = Random.Range(0, 360);
            positionText.text = desiredAngle.ToString();
        }
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
        else if (Input.GetKey(rightTurn) && !Input.GetKeyDown(leftTurn))
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

        transform.rotation = Quaternion.Euler(0, 0, - 1 * currentTurnAmount % 360);
    }

    private void Sink()
    {
        if (shouldSink)
        {
            Sinking();
        }
    }
}

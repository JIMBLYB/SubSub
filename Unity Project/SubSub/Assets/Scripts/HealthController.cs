using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public static int health = 101;
    [SerializeField]
    private int healthLossRate;

    [Header("Debugging")]
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private GameObject deathPanel;

    private void Start()
    {
        Sink();
        Valve.Sinking += Sink;
    }

    private void Sink()
    {
        health -= healthLossRate;
        healthText.text = "Health Remaining: " + health;

        if (health <= 0)
        {
            Sunken();
        }
    }

    private void Sunken()
    {
        deathPanel.SetActive(true);
    }
}

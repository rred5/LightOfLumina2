using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameClock : MonoBehaviour
{
    public TextMeshProUGUI clockText; // Reference to the UI Text component to display the time
    private float elapsedTime = 0f; // Variable to store the elapsed time

    void Update()
    {
        // Increment the elapsed time by the time passed since the last frame
        elapsedTime += Time.deltaTime;

        // Update the UI text to display the elapsed time in seconds
        clockText.text = "Time: " + Mathf.FloorToInt(elapsedTime).ToString() + "s";
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void ResetClock()
    {
        elapsedTime = 0f;
    }
}




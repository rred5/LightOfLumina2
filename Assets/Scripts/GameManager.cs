using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float playerMoney = 0f; // Start with 0 USD
    public float initialGrant = 10000f; // Initial grant received after FBLA meeting

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Call this method when the player attends the FBLA meeting
        AttendFBLAMeeting();
    }

    public void AdjustMoney(float amount)
    {
        playerMoney += amount;
        // Update UI or other systems here if needed
    }

    private void AttendFBLAMeeting()
    {
        // The player receives an initial grant after attending the FBLA meeting
        AdjustMoney(initialGrant);
    }
}





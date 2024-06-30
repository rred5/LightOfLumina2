using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float playerMoney = 0f; // Start with 0 USD
    public int Score;
    public bool allowFinish;

    public GameObject lossMenu; // Reference to the loss menu UI
    public TMPro.TextMeshProUGUI lossText; // Reference to the loss text in the menu
    public UnityEngine.UI.Button retryButton; // Reference to the retry button
    public UnityEngine.UI.Button quitButton; // Reference to the quit button

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
        // Hide the loss menu at the start
        if (lossMenu != null)
        {
            lossMenu.SetActive(false);
        }
    }

    public void AdjustMoney(float amount)
    {
        playerMoney += amount;
        // Update UI or other systems here if needed

        if (playerMoney <= 0)
        {
            TriggerLossCondition();
        }
    }

    private void TriggerLossCondition()
    {
        // Show the loss menu
        if (lossMenu != null)
        {
            Time.timeScale = 0f;
            lossMenu.SetActive(true);
            lossText.text = "You ran out of money! You lose!";
        }

        // Add listeners to buttons
        /*
        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RetryLevel);

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(QuitGame);
        */
    }

    /*
    private void RetryLevel()
    {
        // Reload the current level
        string currentLevelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentLevelName);

        // Reset player's money and other relevant game states
        playerMoney = 10000f; // or some starting amount
        Score = 0;
        allowFinish = true;

        // Hide the loss menu
        if (lossMenu != null)
        {
            lossMenu.SetActive(false);
        }
    }

    private void QuitGame()
    {
        // Implement your quit game logic here
        Application.Quit();
    }
    */
}




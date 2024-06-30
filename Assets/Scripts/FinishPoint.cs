using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    public GameObject finishLevelMenu; // Reference to the finish level menu UI
    public TextMeshProUGUI levelCompleteText; // Reference to the level complete text in the menu
    public TextMeshProUGUI scoreText; // Reference to the score text in the menu
    public Button nextLevelButton; // Reference to the next level button
    public Button quitButton; // Reference to the quit button
    public bool multiplayer = false; // Whether the game is in multiplayer mode
    private bool levelCompleted = false;

    public int goalAmount = 0;

    void Start()
    {
        // Ensure the finish level menu is hidden at the start
        finishLevelMenu.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            // Check if player has met certain conditions
            if (PlayerMeetsConditions())
            {
                Time.timeScale = 0f;
                levelCompleted = true;
                CompleteLevel();
            }
        }
    }

    private bool PlayerMeetsConditions()
    {
        // Implement your logic to check if the player has met certain conditions
        // For example, collected all items or reached a certain score

        if ((GameManager.Instance.playerMoney >= goalAmount) && (GameManager.Instance.allowFinish))
        {
            return true;    
        }
        else { return false; }
        
       // Replace with actual condition checking
    }

    private void CompleteLevel()
    {
        Time.timeScale = 1f;

        if (!multiplayer)
        {
            string currentLevelName = SceneManager.GetActiveScene().name;
            int currentScore = GameManager.Instance.Score; // Assuming GameManager has a GetCurrentScore method

            // Update the scoreboard if the score is within the top 5 scores
            SaveScore(currentLevelName, currentScore);

            // Display the finish level menu with next level option
            finishLevelMenu.SetActive(true);
            levelCompleteText.text = currentLevelName + " Complete!";
            scoreText.text = "Score: " + currentScore.ToString(); // Display the score
            string nextLevelName = GetNextLevelName(currentLevelName);
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(() => LoadNextLevel(nextLevelName));
            nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Level: " + nextLevelName;
            nextLevelButton.gameObject.SetActive(true);
        }
        else
        {
            // Display the finish level menu with only the quit option
            finishLevelMenu.SetActive(true);
            levelCompleteText.text = "Multiplayer Mode - Level Complete!";
            scoreText.text = ""; // No score text in multiplayer mode
            nextLevelButton.gameObject.SetActive(false);
        }

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(QuitGame);
    }

    private void SaveScore(string levelName, int score)
    {
        string key = levelName + "Score";
        int[] topScores = new int[5];

        // Retrieve existing scores
        for (int i = 0; i < 5; i++)
        {
            topScores[i] = PlayerPrefs.GetInt(key + i, 0);
        }

        // Add the new score and sort the array
        for (int i = 0; i < 5; i++)
        {
            if (score > topScores[i])
            {
                for (int j = 4; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                }
                topScores[i] = score;
                break;
            }
        }

        // Save the updated scores
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(key + i, topScores[i]);
        }
        PlayerPrefs.Save();
    }

    private string GetNextLevelName(string currentLevelName)
    {
        int currentLevelNumber;
        if (int.TryParse(currentLevelName.Replace("Level", ""), out currentLevelNumber))
        {
            int nextLevelNumber = currentLevelNumber + 1;
            return "Level" + nextLevelNumber;
        }
        return "Level1"; // Default to Level1 if parsing fails
    }

    private void LoadNextLevel(string nextLevelName)
    {
        SceneManager.LoadScene(nextLevelName);
    }

    private void QuitGame()
    {
        // Implement your quit game logic here
        Application.Quit();
    }
}




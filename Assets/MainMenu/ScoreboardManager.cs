using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardManager : MonoBehaviour
{
    [System.Serializable]
    public class Scoreboard
    {
        public string title;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI[] positionTexts; // Should be 5 elements
        public TextMeshProUGUI[] scoreTexts; // Should be 5 elements
        public string playerPrefKey;
        public GameObject panel; // The panel associated with this scoreboard
    }

    public Scoreboard[] scoreboards;
    public Button nextButton;
    public Button previousButton;
    private int currentScoreboardIndex = 0;

    void Start()
    {
        foreach (var scoreboard in scoreboards)
        {
            scoreboard.titleText.text = scoreboard.title;
            DisplayScores(scoreboard);
            scoreboard.panel.SetActive(false); // Hide all panels initially
        }

        ShowScoreboard(currentScoreboardIndex); // Show the first scoreboard

        // Set up button click listeners
        nextButton.onClick.AddListener(ShowNextScoreboard);
        previousButton.onClick.AddListener(ShowPreviousScoreboard);
    }

    public void SaveScore(int scoreboardIndex, int score)
    {
        string key = scoreboards[scoreboardIndex].playerPrefKey;
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

        // Display the updated scores
        DisplayScores(scoreboards[scoreboardIndex]);
    }

    void DisplayScores(Scoreboard scoreboard)
    {
        string key = scoreboard.playerPrefKey;

        for (int i = 0; i < 5; i++)
        {
            int score = PlayerPrefs.GetInt(key + i, 0);
            scoreboard.positionTexts[i].text = (i + 1).ToString() + "th";
            scoreboard.scoreTexts[i].text = score.ToString();
        }
    }

    void ShowScoreboard(int index)
    {
        // Hide all scoreboards
        foreach (var scoreboard in scoreboards)
        {
            scoreboard.panel.SetActive(false);
        }

        // Show the selected scoreboard
        scoreboards[index].panel.SetActive(true);

        // Update button states
        nextButton.interactable = index < scoreboards.Length - 1;
        previousButton.interactable = index > 0;
    }

    void ShowNextScoreboard() { currentScoreboardIndex = (currentScoreboardIndex + 1) % scoreboards.Length; ShowScoreboard(currentScoreboardIndex); }
    void ShowPreviousScoreboard() { currentScoreboardIndex = (currentScoreboardIndex - 1 + scoreboards.Length) % scoreboards.Length; ShowScoreboard(currentScoreboardIndex); }

    public void AddScoreViaButton(string scoreString)
    {
        int score;
        if (int.TryParse(scoreString, out score))
        {
            SaveScore(currentScoreboardIndex, score);
        }
        else
        {
            Debug.LogError("Invalid score input");
        }
    }

}




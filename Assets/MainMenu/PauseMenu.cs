using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject inputManagerHead;
    [SerializeField] GameObject firstButton;
    private InputManager inputManager;
    private bool isPaused = false;

    private void Awake()
    {
        inputManager = inputManagerHead.GetComponent<InputManager>();
    }

    private void Update()
    {
        if (inputManager.GetPausePressed())
        {
            TogglePause();
            inputManager.SetPausePressed(false);
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
       Time.timeScale = 0f; // Pause the game
        isPaused = true;
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume normal time scale
        isPaused = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen; // Reference to the Game Over screen
    [SerializeField] private AudioClip gameOverSound;

    private void Awake()
    {
        // Ensure the Game Over screen is disabled at the start
        if (gameOverScreen == null)
        {
            Debug.LogError("Game Over screen is not assigned in the UIManager.");
        }
        else
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Activate the Game Over screen
            SoundManager.instance.PlaySound(gameOverSound); // Play Game Over sound
            Debug.Log("Game Over screen activated."); // Log when Game Over screen is shown
        }
        else
        {
            Debug.LogError("Game Over screen is null when trying to activate.");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}










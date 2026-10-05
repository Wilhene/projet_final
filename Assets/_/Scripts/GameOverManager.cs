using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource buttonSound;

    [Header("Scene à recommencer")]
    public string levelToRestart = "SousSol";

    private void Start()
    {
        // Remet le jeu à vitesse normale
        Time.timeScale = 1f;

        // Libère la souris pour le menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        SceneManager.LoadScene(levelToRestart);
    }

    public void GoBackToMenu()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        SceneManager.LoadScene("Accueil");
    }
}
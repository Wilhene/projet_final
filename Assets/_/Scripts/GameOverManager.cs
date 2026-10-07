using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // "static" permet de garder le nom du niveau
    // même lorsqu'on passe à la scène GameOver.
    public static string levelToRestart;

    [Header("Audio")]
    public AudioSource buttonSound;

    private void Start()
    {
        // Remet le jeu à vitesse normale.
        Time.timeScale = 1f;

        // Libère et affiche la souris.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        // Joue le son du bouton.
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        // Vérifie qu'une scène a bien été mémorisée.
        if (!string.IsNullOrEmpty(levelToRestart))
        {
            // Recharge le niveau dans lequel le joueur est mort.
            SceneManager.LoadScene(levelToRestart);
        }
    }

    public void GoBackToMenu()
    {
        // Joue le son du bouton.
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        // Retour au menu principal.
        SceneManager.LoadScene("Accueil");
    }
}
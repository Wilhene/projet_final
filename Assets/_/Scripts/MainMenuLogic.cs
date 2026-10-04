using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public AudioSource buttonSound;

    public void StartButton()
    {
        Debug.Log("NEW GAME CLIQUÉ");

        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        SceneManager.LoadScene("Maison-niveau-1");
    }

    public void ExitGameButton()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }

        Debug.Log("Jeu fermé");
        Application.Quit();
    }
}
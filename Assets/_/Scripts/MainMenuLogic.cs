using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuLogic : MonoBehaviour
{
    // AudioSource contenant le son des boutons
    public AudioSource buttonSound;

    // Joue le son lorsqu'un bouton est cliqué
    public void PlayButtonSound()
    {
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }

    // Lance une nouvelle partie
    public void StartButton()
    {
        PlayButtonSound();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // Attend un peu pour laisser jouer le son
        yield return new WaitForSecondsRealtime(0.2f);

        // Efface l'ancienne progression
        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.ResetProgress();
        }

        // Charge le premier niveau
        SceneManager.LoadScene("Maison-niveau-1");
    }

    // Ferme le jeu
    public void ExitGameButton()
    {
        PlayButtonSound();
        StartCoroutine(QuitGame());
    }

    private IEnumerator QuitGame()
    {
        // Attend un peu avant de fermer le jeu
        yield return new WaitForSecondsRealtime(0.2f);

        Application.Quit();
    }
}
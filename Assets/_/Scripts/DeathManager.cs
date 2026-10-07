using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    // Appelée par le CES lorsque l'Entité touche le joueur.
    public void Die()
    {

        // Mémorise la scène dans laquelle le joueur est mort.
        GameOverManager.levelToRestart = SceneManager.GetActiveScene().name;

        // Va vers l'écran Game Over.
        SceneManager.LoadScene("GameOver");
    }
}
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    [Header("Objets Niveau 1")]
    public bool objet1 = false;
    public bool objet2 = false;

    [Header("Objets Niveau 2")]
    public bool objet3 = false;
    public bool objet4 = false;

    private void Awake()
    {
        // S'il existe déjà un GameProgress, on détruit le nouveau
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Cet objet survit aux changements de scènes
        DontDestroyOnLoad(gameObject);
    }

    public void ResetProgress()
    {
        objet1 = false;
        objet2 = false;
        objet3 = false;
        objet4 = false;

        Debug.Log("Progression réinitialisée !");
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

// À placer sur PlayerCapsule (à côté du First Person Controller)
[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    [Header("Pas")]
    public AudioClip[] sonsDePas;              // Plusieurs sons = plus naturel (choisis au hasard)
    public float intervalleMarche = 0.5f;      // Temps entre deux pas en marchant
    public float intervalleCourse = 0.3f;      // Temps entre deux pas en courant
    [Range(0f, 1f)] public float volumePas = 0.6f;

    [Header("Saut et atterrissage")]
    public AudioClip sonSaut;
    public AudioClip sonAtterrissage;

    [Header("Tir")]
    public AudioClip sonTir;
    [Range(0f, 1f)] public float volumeTir = 0.8f;

    AudioSource source;
    CharacterController cc;
    FirstPersonController controleur;
    StarterAssetsInputs input;

    float minuteriePas;
    bool etaitAuSol = true;
    float vitesseVerticalePrecedente;

    void Start()
    {
        source = GetComponent<AudioSource>();
        cc = GetComponent<CharacterController>();
        controleur = GetComponent<FirstPersonController>();
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        bool auSol = controleur.Grounded;
        float vitesseVerticale = cc.velocity.y;
        Vector3 vitesseHorizontale = new Vector3(cc.velocity.x, 0f, cc.velocity.z);

        // --- Pas (marche et course) ---
        if (auSol && vitesseHorizontale.magnitude > 0.1f)
        {
            minuteriePas -= Time.deltaTime;
            if (minuteriePas <= 0f)
            {
                JouerPas();
                minuteriePas = input.sprint ? intervalleCourse : intervalleMarche;
            }
        }
        else
        {
            minuteriePas = 0f; // Le prochain pas se joue dès qu'on recommence à bouger
        }

        // --- Saut (aussi le double saut et les launch pads) : poussée soudaine vers le haut ---
        if (vitesseVerticale > vitesseVerticalePrecedente + 2f && vitesseVerticale > 1f)
        {
            Jouer(sonSaut, 1f);
        }

        // --- Atterrissage après une vraie chute ---
        if (auSol && !etaitAuSol && vitesseVerticalePrecedente < -3f)
        {
            Jouer(sonAtterrissage, 1f);
        }

        // --- Tir (même bouton que le script Tir) ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Jouer(sonTir, volumeTir);
        }

        etaitAuSol = auSol;
        vitesseVerticalePrecedente = vitesseVerticale;
    }

    void JouerPas()
    {
        if (sonsDePas == null || sonsDePas.Length == 0) return;
        AudioClip clip = sonsDePas[Random.Range(0, sonsDePas.Length)];
        source.PlayOneShot(clip, volumePas);
    }

    void Jouer(AudioClip clip, float volume)
    {
        if (clip != null) source.PlayOneShot(clip, volume);
    }
}
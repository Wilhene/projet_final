
using System.Collections; // Permet d'utiliser les Coroutines
using UnityEngine; // Permet d'utiliser les fonctions de Unity
using UnityEngine.InputSystem; // Permet de détecter les clics de souris

public class SpiritBox : MonoBehaviour
{
    // ==========================================
    // RÉFÉRENCES DU JOUEUR
    // ==========================================

    // Position du joueur dans la scène
    public Transform player;

    // Permet de connaître l'étage actuel du joueur
    public PlayerFloor playerFloor;


    // ==========================================
    // DÉTECTION DES OBJETS PARANORMAUX
    // ==========================================

    [Header("Detection")]

    // Distance maximale de détection de la Spirit Box
    public float detectionRange = 15f;


    // ==========================================
    // AUDIO
    // ==========================================

    [Header("Audio")]

    // Son joué quand aucun objet paranormal n'est détecté
    public AudioSource spiritBoxAudio;


    // ==========================================
    // BOUTON ROUGE DE LA SPIRIT BOX
    // ==========================================

    [Header("Button")]

    // Référence au bouton rouge du modèle 3D
    public Transform button;

    // Distance parcourue par le bouton lorsqu'il est enfoncé
    public float pressDistance = 0.01f;

    // Durée pendant laquelle le bouton reste enfoncé
    public float pressDuration = 0.08f;


    // ==========================================
    // VARIABLES PRIVÉES
    // ==========================================

    // Position de départ du bouton rouge
    private Vector3 buttonStartPosition;

    // Garde une référence à l'animation du bouton
    private Coroutine buttonAnimation;


    // ==========================================
    // DÉMARRAGE DU JEU
    // ==========================================

    void Start()
    {
        // Vérifie qu'un bouton rouge a été assigné
        if (button != null)
        {
            // Enregistre sa position locale de départ
            // pour pouvoir le remettre en place après un clic
            buttonStartPosition = button.localPosition;
        }
    }


    // ==========================================
    // DÉTECTION DU CLIC GAUCHE
    // ==========================================

    void Update()
    {
        // Vérifie que la souris existe et que
        // le joueur vient d'appuyer sur le clic gauche
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Recherche l'objet paranormal le plus proche
            DetectNearestObject();

            // Vérifie qu'un bouton rouge a été assigné
            if (button != null)
            {
                // Si une animation du bouton est déjà en cours,
                // on l'arrête pour éviter les conflits
                if (buttonAnimation != null)
                {
                    StopCoroutine(buttonAnimation);
                }

                // Lance l'animation du bouton rouge
                buttonAnimation = StartCoroutine(PressButton());
            }
        }
    }


    // ==========================================
    // ANIMATION DU BOUTON ROUGE
    // ==========================================

    IEnumerator PressButton()
    {
        // Calcule la position du bouton lorsqu'il est enfoncé
        // Vector3.down = déplacement vers le bas (axe Y négatif)
        // pressDistance = distance du déplacement
        Vector3 pressedPosition =
            buttonStartPosition + Vector3.down * pressDistance;

        // Déplace le bouton rouge vers le bas
        button.localPosition = pressedPosition;

        // Attend quelques instants avant de relâcher le bouton
        yield return new WaitForSeconds(pressDuration);

        // Replace le bouton à sa position de départ
        button.localPosition = buttonStartPosition;

        // Indique que l'animation du bouton est terminée
        buttonAnimation = null;
    }


    // ==========================================
    // DÉTECTION DES OBJETS PARANORMAUX
    // ==========================================

    void DetectNearestObject()
    {
        // Récupère tous les objets ayant le tag ParanormalObject
        GameObject[] paranormalObjects =
            GameObject.FindGameObjectsWithTag("ParanormalObject");

        // Variable qui contiendra l'objet le plus proche
        GameObject nearestObject = null;

        // Au départ, aucune distance n'a encore été trouvée
        float nearestDistance = Mathf.Infinity;


        // Parcourt tous les objets paranormaux trouvés
        foreach (GameObject paranormalObject in paranormalObjects)
        {
            // Récupère le script ParanormalObject
            // pour connaître les informations de cet objet
            ParanormalObject objectInfo =
                paranormalObject.GetComponent<ParanormalObject>();

            // Si l'objet n'a pas le script ParanormalObject,
            // on passe directement au prochain objet
            if (objectInfo == null)
            {
                continue;
            }

            // Vérifie si l'objet est au même étage que le joueur
            // Si ce n'est pas le cas, on l'ignore
            if (objectInfo.floor != playerFloor.currentFloor)
            {
                continue;
            }

            // Calcule la distance entre le joueur et l'objet
            float distance = Vector3.Distance(
                player.position,
                paranormalObject.transform.position
            );

            // Vérifie si cet objet est plus proche
            // que celui trouvé précédemment
            if (distance < nearestDistance)
            {
                // Enregistre la nouvelle distance
                nearestDistance = distance;

                // Enregistre l'objet le plus proche
                nearestObject = paranormalObject;
            }
        }


        // ==========================================
        // SI UN OBJET PARANORMAL EST DÉTECTÉ
        // ==========================================

        // Vérifie qu'un objet a été trouvé
        // et qu'il est à 15 mètres ou moins
        if (nearestObject != null &&
            nearestDistance <= detectionRange)
        {
            // Récupère l'AudioSource de l'objet paranormal
            AudioSource objectAudio =
                nearestObject.GetComponent<AudioSource>();

            // Vérifie si l'objet possède un son
            if (objectAudio != null)
            {
                // Joue le son de l'objet paranormal
                objectAudio.Play();
            }

            // Affiche les informations dans la Console Unity
            // pour vérifier quel objet a été détecté
            Debug.Log(
                "Objet détecté : "
                + nearestObject.name
                + " | Étage : "
                + playerFloor.currentFloor
                + " | Distance : "
                + nearestDistance
            );
        }


        // ==========================================
        // SI AUCUN OBJET N'EST DÉTECTÉ
        // ==========================================

        else
        {
            // Vérifie que la Spirit Box possède un AudioSource
            if (spiritBoxAudio != null)
            {
                // Joue le bruit de parasites de la Spirit Box
                spiritBoxAudio.Play();
            }

            // Affiche un message dans la Console Unity
            Debug.Log(
                "Aucune réponse à l'étage "
                + playerFloor.currentFloor
            );
        }
    }
}

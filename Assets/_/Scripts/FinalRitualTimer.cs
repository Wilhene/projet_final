// Permet d'utiliser les fonctions principales de Unity
// comme GameObject, MonoBehaviour, Time, Debug, etc.
using UnityEngine;

// Permet d'utiliser TextMeshPro pour afficher notre timer à l'écran.
using TMPro;


// Création de notre script FinalRitualTimer.
// MonoBehaviour permet à ce script d'être placé sur un GameObject dans Unity.
public class FinalRitualTimer : MonoBehaviour
{

    // =========================
    // TIMER
    // =========================

    // Crée une catégorie "Timer" dans l'Inspector.
    [Header("Timer")]

    // Durée de la phase de survie.
    // "public" permet de modifier cette valeur dans l'Inspector.
    public float survivalTime = 60f;


    // =========================
    // INTERFACE
    // =========================

    // Crée une catégorie "UI" dans l'Inspector.
    [Header("UI")]

    // Référence vers le texte TextMeshPro qui affichera le temps.
    public TMP_Text timerText;


    // =========================
    // ENTITÉ
    // =========================

    // Crée une catégorie "Entité" dans l'Inspector.
    [Header("Entité")]

    // Référence vers l'Entité présente dans le sanctuaire.
    // On va utiliser cette référence pour la faire disparaître
    // lorsque le joueur survit aux 60 secondes.
    public GameObject entity;


    // =========================
    // FIN DU RITUEL
    // =========================

    // Crée une catégorie "Fin du rituel" dans l'Inspector.
    [Header("Fin du rituel")]

    // Référence vers la porte qui bloque la sortie finale.
    public GameObject finalDoor;


    // =========================
    // VARIABLES INTERNES
    // =========================

    // Contient le temps restant actuellement.
    // "private" = seulement ce script peut utiliser cette variable.
    private float timer;

    // Indique si la phase finale de 60 secondes a commencé.
    //
    // false = pas commencée
    // true = commencée
    private bool ritualStarted = false;

    // Indique si le joueur a terminé les 60 secondes.
    //
    // false = pas terminé
    // true = terminé
    private bool ritualFinished = false;


    // =========================
    // START
    // =========================

    // Start() est exécuté UNE FOIS lorsque ce GameObject apparaît.
    private void Start()
    {
        // On donne au timer sa valeur de départ.
        // Exemple : si survivalTime = 60,
        // alors timer devient également 60.
        timer = survivalTime;


        // Vérifie qu'un Timer Text a bien été assigné.
        // != null veut dire "il existe".
        if (timerText != null)
        {
            // Cache le texte du timer au début du niveau.
            //
            // Le joueur est encore dans la partie où
            // il doit trouver et détruire les sceaux,
            // donc on ne veut pas encore afficher le countdown.
            timerText.gameObject.SetActive(false);
        }
    }


    // =========================
    // UPDATE
    // =========================

    // Update() est exécuté à CHAQUE FRAME du jeu.
    private void Update()
    {
        // Si la phase finale n'a PAS commencé
        // OU si elle est déjà terminée...
        if (!ritualStarted || ritualFinished)

            // ...on arrête Update ici.
            // Le timer ne descend donc pas.
            return;


        // Retire du temps au timer.
        //
        // Time.deltaTime représente le temps écoulé
        // depuis la dernière frame.
        //
        // Ça permet au timer de descendre en vraies secondes.
        timer -= Time.deltaTime;


        // Empêche le timer de descendre sous zéro.
        if (timer < 0f)

            // Si timer devient négatif,
            // on le remet simplement à 0.
            timer = 0f;


        // Appelle notre fonction qui actualise
        // le texte du timer à l'écran.
        UpdateTimerUI();


        // Vérifie si le timer est arrivé à zéro.
        if (timer <= 0f)
        {
            // Si oui, le joueur a survécu
            // et on termine le rituel.
            FinishRitual();
        }
    }


    // =========================
    // COMMENCER LES 60 SECONDES
    // =========================

    // Fonction publique qui commence la phase finale.
    //
    // Comme elle est "public",
    // notre CES peut appeler cette fonction.
    //
    // IMPORTANT :
    // Dans notre jeu, cette fonction est appelée
    // APRÈS que les 3 sceaux ont été détruits.
    public void StartRitual()
    {
        // Vérifie si la phase finale a déjà commencé.
        if (ritualStarted)

            // Si oui, on arrête ici.
            // Cela empêche de lancer le timer plusieurs fois.
            return;


        // La phase finale est maintenant commencée.
        ritualStarted = true;


        // Remet le timer à sa durée complète.
        // Exemple : 60 secondes.
        timer = survivalTime;


        // Vérifie que le texte existe.
        if (timerText != null)
        {
            // Rend le timer visible à l'écran.
            timerText.gameObject.SetActive(true);
        }


        // Actualise immédiatement le texte.
        // Le joueur verra donc directement :
        //
        // SURVIVE
        // 60
        UpdateTimerUI();


        // Écrit ce message dans la Console de Unity.
        // C'est simplement utile pour vérifier
        // que StartRitual() a bien été déclenché.
        Debug.Log("PHASE FINALE : SURVIVEZ 60 SECONDES");
    }


    // =========================
    // AFFICHAGE DU TIMER
    // =========================

    // Cette fonction s'occupe uniquement
    // de modifier le texte affiché à l'écran.
    private void UpdateTimerUI()
    {
        // Vérifie que le TextMeshPro existe.
        if (timerText != null)
        {
            // Change ce qui est écrit dans le TextMeshPro.
            //
            // "\n" veut dire :
            // aller à la ligne.
            //
            // Donc :
            //
            // SURVIVE
            // 60
            //
            //
            // Mathf.CeilToInt arrondit vers le haut.
            //
            // Par exemple :
            //
            // 59.8 → 60
            // 42.3 → 43
            // 10.1 → 11
            //
            // Ça évite d'afficher quelque chose comme :
            // 59.827364 secondes.
            timerText.text = "SURVIVE\n" + Mathf.CeilToInt(timer);
        }
    }


    // =========================
    // FIN DU RITUEL
    // =========================

    // Cette fonction est appelée automatiquement
    // lorsque le timer atteint zéro.
    private void FinishRitual()
    {
        // Indique que le rituel est maintenant terminé.
        //
        // Grâce à ça, Update() arrêtera
        // de faire descendre le timer.
        ritualFinished = true;


        // Vérifie que notre texte existe.
        if (timerText != null)
        {
            // Remplace le countdown par un message final.
            timerText.text = "RITUAL COMPLETE";
        }


        // =========================
        // DÉSACTIVATION DE L'ENTITÉ
        // =========================

        // Vérifie qu'une Entité a bien été assignée
        // dans l'Inspector.
        if (entity != null)
        {
            // Désactive complètement le GameObject de l'Entité.
            //
            // Son modèle, son animation, son CES de mort,
            // etc. sont donc désactivés avec elle.
            //
            // Le joueur ne peut donc plus mourir
            // en touchant l'Entité après la fin du rituel.
            entity.SetActive(false);
        }


        // =========================
        // OUVERTURE DE LA SORTIE
        // =========================

        // Vérifie qu'une porte a été assignée
        // dans l'Inspector.
        if (finalDoor != null)
        {
            // Désactive complètement la porte.
            //
            // Elle disparaît donc
            // et le joueur peut passer.
            finalDoor.SetActive(false);
        }


        // Message de vérification dans la Console Unity.
        //
        // Si tu vois ce message,
        // ça veut dire que les 60 secondes
        // ont bien été terminées.
        Debug.Log("RITUEL TERMINÉ !");
    }
}
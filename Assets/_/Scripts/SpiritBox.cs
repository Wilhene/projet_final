using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritBox : MonoBehaviour
{
    public Transform player;
    public PlayerFloor playerFloor;

    [Header("Detection")]
    public float detectionRange = 15f;

    [Header("Audio")]
    public AudioSource spiritBoxAudio;

    [Header("Button")]
    public Transform button;
    public float pressDistance = 0.01f;
    public float pressDuration = 0.08f;

    private Vector3 buttonStartPosition;
    private Coroutine buttonAnimation;

    void Start()
    {
        if (button != null)
        {
            buttonStartPosition = button.localPosition;
        }
    }

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            DetectNearestObject();

            if (button != null)
            {
                if (buttonAnimation != null)
                {
                    StopCoroutine(buttonAnimation);
                }

                buttonAnimation = StartCoroutine(PressButton());
            }
        }
    }

    IEnumerator PressButton()
    {
        Vector3 pressedPosition =
            buttonStartPosition + Vector3.left * pressDistance;

        button.localPosition = pressedPosition;

        yield return new WaitForSeconds(pressDuration);

        button.localPosition = buttonStartPosition;

        buttonAnimation = null;
    }

    void DetectNearestObject()
    {
        GameObject[] paranormalObjects =
            GameObject.FindGameObjectsWithTag("ParanormalObject");

        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject paranormalObject in paranormalObjects)
        {
            // Récupère les informations de l'objet paranormal
            ParanormalObject objectInfo =
                paranormalObject.GetComponent<ParanormalObject>();

            // Si l'objet n'a pas le script ParanormalObject,
            // on l'ignore
            if (objectInfo == null)
            {
                continue;
            }

            // Si l'objet n'est pas au même étage que le joueur,
            // on l'ignore aussi
            if (objectInfo.floor != playerFloor.currentFloor)
            {
                continue;
            }

            // Seulement maintenant on calcule sa distance
            float distance = Vector3.Distance(
                player.position,
                paranormalObject.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestObject = paranormalObject;
            }
        }

        // Objet du BON étage trouvé dans les 15 m
        if (nearestObject != null &&
            nearestDistance <= detectionRange)
        {
            AudioSource objectAudio =
                nearestObject.GetComponent<AudioSource>();

            if (objectAudio != null)
            {
                objectAudio.Play();
            }

            Debug.Log(
                "Objet détecté : "
                + nearestObject.name
                + " | Étage : "
                + playerFloor.currentFloor
                + " | Distance : "
                + nearestDistance
            );
        }

        // Aucun objet du bon étage dans les 15 m
        else
        {
            if (spiritBoxAudio != null)
            {
                spiritBoxAudio.Play();
            }

            Debug.Log(
                "Aucune réponse à l'étage "
                + playerFloor.currentFloor
            );
        }
    }
}
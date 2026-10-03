using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpiritBox : MonoBehaviour
{
    public Transform player;

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

        // Le bouton s'enfonce
        button.localPosition = pressedPosition;

        yield return new WaitForSeconds(pressDuration);

        // Le bouton revient
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
                + " | Distance : "
                + nearestDistance
            );
        }
        else
        {
            if (spiritBoxAudio != null)
            {
                spiritBoxAudio.Play();
            }

            Debug.Log("Aucune réponse.");
        }
    }
}
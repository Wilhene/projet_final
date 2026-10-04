using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    [Header("Floor")]
    public int floorToSet;

    private void OnTriggerEnter(Collider other)
    {
        PlayerFloor playerFloor = other.GetComponent<PlayerFloor>();

        if (playerFloor != null)
        {
            playerFloor.currentFloor = floorToSet;

            Debug.Log("Joueur maintenant à l'étage : " + floorToSet);
        }
    }
}
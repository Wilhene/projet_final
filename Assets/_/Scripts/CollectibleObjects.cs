using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public bool collected = false;

    public void Collect()
    {
        if (collected)
            return;

        collected = true;

        Debug.Log(gameObject.name + " collecté !");

        gameObject.SetActive(false);
    }
}
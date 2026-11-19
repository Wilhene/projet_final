using UnityEngine;

[RequireComponent(typeof(Light))]
public class LumiereVacillante : MonoBehaviour
{
    public float intensiteMin = 0.6f;
    public float intensiteMax = 1.4f;
    public float vitesse = 7f;

    Light lumiere;
    float decalage;

    void Start()
    {
        lumiere = GetComponent<Light>();
        decalage = Random.Range(0f, 100f);
    }

    void Update()
    {
        float bruit = Mathf.PerlinNoise(decalage, Time.time * vitesse);
        lumiere.intensity = Mathf.Lerp(intensiteMin, intensiteMax, bruit);
    }
}
using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField]private Material mat;

    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float emissionStrength = 5f;


    public void Flash()
    {
        StopCoroutine(FlashRoutine());
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        // Enable emission
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.white * emissionStrength);

        yield return new WaitForSeconds(flashDuration);

        // Turn off emission
        mat.SetColor("_EmissionColor", Color.black);
        mat.DisableKeyword("_EMISSION");
    }
}
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

// Code from farmageddon
public class FlashEffect : MonoBehaviour
{
    [Header("References")]
    private Material[] materials;
    private Image[] image;

    [Header("Flash Settings")]
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTimer = 0.1f;


    private void Awake()
    {
        image = GetComponentsInChildren<Image>();
        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        materials = new Material[image.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = Instantiate(image[i].material);
            image[i].material = materials[i];
        }
    }

    /// <summary>
    /// Triggers a flash effect.
    /// Optional: 
    /// onFlashStart > called immediately when flash starts.
    /// onFlashComplete > called when flash finishes.
    /// </summary>
    public void CallDamageFlash()
    {
        StopAllCoroutines(); // stop any ongoing flash
        StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        // Set flash color
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", flashColor);
        }

        // Lerp flash from 1 to 0 over flashTimer
        float elapsedTime = 0f;
        while (elapsedTime < flashTimer)
        {
            elapsedTime += Time.deltaTime;
            float time = Mathf.Clamp01(elapsedTime / flashTimer);
            float flashAmount = Mathf.Lerp(1f, 0f, time);

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetFloat("_FlashAmount", flashAmount);
            }

            yield return null;
        }

        // Ensure flash ends at 0
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmount", 0f);
        }
        //Debug.Log("flash called");
    }
}
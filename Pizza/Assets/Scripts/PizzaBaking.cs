using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PizzaBaking : MonoBehaviour
{
    public Material bakedPizzaMaterial;
    public Material bakedCheeseMaterial;
    public Material bakedSausageMaterial;
    public Material bakedTomatoMaterial;
    private Color ketchupBakedColor = new Color32(184, 67, 67, 250);
    private float duration = 2.0f;
    private bool hasBaked = false;

    public void StartBaking()
    {
        if (hasBaked) return;
        hasBaked = true;

        GameObject pizza = PizzaStageManager.Instance.GetCurrentPizza();
        if (pizza == null)
        {
            Debug.LogError("Текущая пицца не найдена");
            return;
        }

        Renderer pizzaRenderer = pizza.GetComponent<Renderer>();
        if (pizzaRenderer != null && bakedPizzaMaterial != null)
        {
            StartCoroutine(ChangeMaterial(pizzaRenderer, bakedPizzaMaterial));
        }

        Renderer[] allRenderers = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in allRenderers)
        {
            if (r == pizzaRenderer) continue;

            Material matToApply = GetMaterialByTag(r.gameObject.tag);
            if (matToApply != null)
            {
                StartCoroutine(ChangeMaterial(r, matToApply));
            }
        }

        SpriteRenderer ketchup = pizza.GetComponentInChildren<SpriteRenderer>();
        if (ketchup != null)
        {
            StartCoroutine(ChangeSpriteColor(ketchup, ketchupBakedColor));
        }
    }

    private Material GetMaterialByTag(string tag)
    {
        switch (tag)
        {
            case "Cheese": return bakedCheeseMaterial;
            case "Sausage": return bakedSausageMaterial;
            case "Tomato": return bakedTomatoMaterial;
            default: return null;
        }
    }

    private IEnumerator ChangeMaterial(Renderer renderer, Material targetMaterial)
    {
        Material original = renderer.material;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            renderer.material.Lerp(original, targetMaterial, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        renderer.material = targetMaterial;
    }

    private IEnumerator ChangeSpriteColor(SpriteRenderer spriteRenderer, Color targetColor)
    {
        Color originalColor = spriteRenderer.color;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
    }
}
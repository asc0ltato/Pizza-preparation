using UnityEngine;

public class PizzaStageManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject doughPrefab;
    [HideInInspector] public GameObject currentPizza;

    private static PizzaStageManager instance;
    public static PizzaStageManager Instance => instance;

    private void Awake()
    {
        instance = this;
        SpawnNewPizza();
    }

    public void SpawnNewPizza()
    {
        currentPizza = Instantiate(doughPrefab, spawnPoint.position, spawnPoint.rotation);

        SaucePouring saucePouring = FindFirstObjectByType<SaucePouring>();
        if (saucePouring != null)
        {
            saucePouring.RefreshKetchupSpotReference();
        }
    }

    public void ClearCurrentPizzaReference()
    {
        currentPizza = null;
    }

    public GameObject GetCurrentPizza()
    {
        return currentPizza;
    }

    public Transform GetCurrentPizzaTransform()
    {
        return currentPizza != null ? currentPizza.transform : null;
    }
}
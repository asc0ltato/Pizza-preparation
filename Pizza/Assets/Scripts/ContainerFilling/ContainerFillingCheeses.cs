using System.Collections.Generic;
using UnityEngine;

public class ContainerFillingCheeses : MonoBehaviour
{
    public GameObject cheese;
    public BoxCollider containerCollider;

    [HideInInspector]
    public List<GameObject> cheeses = new List<GameObject>();

    private int countCheeses = 20;
    private float spacing = 1f;

    void Start()
    {
        FillContainerWithCheeses();
    }

    void FillContainerWithCheeses()
    {
        Vector3 containerSize = containerCollider.size;
        Vector3 containerCenter = containerCollider.center;
        Renderer cheeseRenderer = cheese.GetComponent<Renderer>();
        Vector3 cheeseSize = cheeseRenderer.bounds.size;

        Vector3 step = new Vector3(
            (containerSize.x - cheeseSize.x) / (countCheeses - 1),
            (containerSize.y - cheeseSize.y) / (countCheeses - 1),
            (containerSize.z - cheeseSize.z) / (countCheeses - 1)
        ) * spacing;

        for (int x = 0; x < countCheeses; x++)
        {
            for (int y = 0; y < countCheeses; y++)
            {
                for (int z = 0; z < countCheeses; z++)
                {
                    Vector3 localPos = new Vector3(
                        -containerSize.x / 2f + x * step.x + cheeseSize.x / 2f,
                        -containerSize.y / 2f + y * step.y + cheeseSize.y / 2f,
                        -containerSize.z / 2f + z * step.z + cheeseSize.z / 2f
                    ) + containerCenter;

                    Vector3 worldPos = containerCollider.transform.TransformPoint(localPos);

                    if (containerCollider.ClosestPoint(worldPos) == worldPos)
                    {
                        GameObject s = Instantiate(cheese, worldPos, containerCollider.transform.rotation);
                        cheeses.Add(s);
                    }
                }
            }
        }

        Debug.Log($"Количество сыра в контейнере: {cheeses.Count}");
    }
}
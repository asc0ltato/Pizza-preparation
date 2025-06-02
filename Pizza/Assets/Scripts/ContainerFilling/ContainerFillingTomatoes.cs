using System.Collections.Generic;
using UnityEngine;

public class ContainerFillingTomatoes : MonoBehaviour
{
    public GameObject tomato;
    public BoxCollider containerCollider;

    [HideInInspector]
    public List<GameObject> tomatoes = new List<GameObject>();

    private int countTomatoes = 7;
    private float spacing = 1f;

    void Start()
    {
        FillContainerWithTomatoes();
    }

    void FillContainerWithTomatoes()
    {
        Vector3 containerSize = containerCollider.size;
        Vector3 containerCenter = containerCollider.center;
        Renderer tomatoRenderer = tomato.GetComponent<Renderer>();
        Vector3 tomatoSize = tomatoRenderer.bounds.size;

        Vector3 step = new Vector3(
            (containerSize.x - tomatoSize.x) / (countTomatoes - 1),
            (containerSize.y - tomatoSize.y) / (countTomatoes - 1),
            (containerSize.z - tomatoSize.z) / (countTomatoes - 1)
        ) * spacing;

        for (int x = 0; x < countTomatoes; x++)
        {
            for (int y = 0; y < countTomatoes; y++)
            {
                for (int z = 0; z < countTomatoes; z++)
                {
                    Vector3 localPos = new Vector3(
                        -containerSize.x / 2f + x * step.x + tomatoSize.x / 2f,
                        -containerSize.y / 2f + y * step.y + tomatoSize.y / 2f,
                        -containerSize.z / 2f + z * step.z + tomatoSize.z / 2f
                    ) + containerCenter;

                    Vector3 worldPos = containerCollider.transform.TransformPoint(localPos);

                    if (containerCollider.ClosestPoint(worldPos) == worldPos)
                    {
                        GameObject s = Instantiate(tomato, worldPos, containerCollider.transform.rotation);
                        tomatoes.Add(s);
                    }
                }
            }
        }

        Debug.Log($"Количество томатов в контейнере: {tomatoes.Count}");
    }
}
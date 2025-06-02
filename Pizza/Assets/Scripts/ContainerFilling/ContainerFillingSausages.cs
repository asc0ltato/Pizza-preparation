using System.Collections.Generic;
using UnityEngine;

public class ContainerFillingSausages : MonoBehaviour
{
    public GameObject sausage;
    public BoxCollider containerCollider;

    [HideInInspector]
    public List<GameObject> sausages = new List<GameObject>();

    private int countSausages = 7;
    private float spacing = 1f;

    void Start()
    {
        FillContainerWithSausages();
    }

    void FillContainerWithSausages()
    {
        Vector3 containerSize = containerCollider.size;
        Vector3 containerCenter = containerCollider.center;
        Renderer sausageRenderer = sausage.GetComponent<Renderer>();
        Vector3 sausageSize = sausageRenderer.bounds.size;

        Vector3 step = new Vector3(
            (containerSize.x - sausageSize.x) / (countSausages - 1),
            (containerSize.y - sausageSize.y) / (countSausages - 1),
            (containerSize.z - sausageSize.z) / (countSausages - 1)
        ) * spacing;

        for (int x = 0; x < countSausages; x++)
        {
            for (int y = 0; y < countSausages; y++)
            {
                for (int z = 0; z < countSausages; z++)
                {
                    Vector3 localPos = new Vector3(
                        -containerSize.x / 2f + x * step.x + sausageSize.x / 2f,
                        -containerSize.y / 2f + y * step.y + sausageSize.y / 2f,
                        -containerSize.z / 2f + z * step.z + sausageSize.z / 2f
                    ) + containerCenter;

                    Vector3 worldPos = containerCollider.transform.TransformPoint(localPos);

                    if (containerCollider.ClosestPoint(worldPos) == worldPos)
                    {
                        GameObject s = Instantiate(sausage, worldPos, containerCollider.transform.rotation);
                        sausages.Add(s);
                    }
                }
            }
        }

        Debug.Log($"Количество колбас в контейнере: {sausages.Count}");
    }
}
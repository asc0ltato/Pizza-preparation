using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragFood : MonoBehaviour
{
    private Vector3 offset;
    public new Camera camera;
    public CameraSwitcher cameraSwitcher;

    private static List<DragFood> selectedCheeses = new List<DragFood>();
    private static Dictionary<DragFood, Vector3> randomOffsets = new Dictionary<DragFood, Vector3>();
    private static Dictionary<DragFood, float> randomRotations = new Dictionary<DragFood, float>();

    private const int maxCheesesToSelect = 5;
    private const float offsetRangeX = 0.03f;
    private const float offsetRangeZ = 0.03f;
    private const float hoverHeight = 0.031f;
    public int stepToUnlock = 4;

    public bool isCheese = false;
    public bool isSausage = false;
    public bool isTomato = false;
    private bool isDragging = false;
    private float initialHeight;

    void OnMouseDown()
    {
        if (!IsOverheadCameraActive()) return;

        isDragging = true;
        initialHeight = transform.position.y;

        Vector3 screenPoint = camera.WorldToScreenPoint(transform.position);
        Vector3 mousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 worldPoint = camera.ScreenToWorldPoint(mousePoint);
        offset = transform.position - worldPoint;

        if (isCheese)
        {
            ClearSelection();

            DragFood[] allCheeses = Object.FindObjectsByType<DragFood>(FindObjectsSortMode.None)
                .Where(f => f.isCheese).ToArray();

            var nearestCheeses = allCheeses
                .OrderBy(f => Vector3.Distance(f.transform.position, transform.position))
                .Take(maxCheesesToSelect)
                .ToList();

            selectedCheeses.AddRange(nearestCheeses);

            randomOffsets.Clear();
            randomRotations.Clear();
            foreach (var cheese in selectedCheeses)
            {
                float randomX = Random.Range(-offsetRangeX, offsetRangeX);
                float randomZ = Random.Range(-offsetRangeZ, offsetRangeZ);
                randomOffsets[cheese] = new Vector3(randomX, 0f, randomZ);

                float randomAngle = Random.Range(-50f, 50f);
                randomRotations[cheese] = randomAngle;

                cheese.transform.rotation = Quaternion.Euler(-90f, randomAngle, 0f);
            }
        }
        else if (isSausage || isTomato)
        {
            ClearSelection();
            selectedCheeses.Add(this);
            initialHeight = transform.position.y;

            randomOffsets.Clear();
            randomRotations.Clear();
            randomOffsets[this] = Vector3.zero;
            randomRotations[this] = 0f;
        }
        else
        {
            ClearSelection();
        }
    }

    private bool IsOverheadCameraActive()
    {
        return cameraSwitcher != null && cameraSwitcher.IsOverheadCameraActive();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.WorldToScreenPoint(transform.position).z);
            Vector3 worldPoint = camera.ScreenToWorldPoint(screenPoint) + offset;

            float targetHeight = initialHeight + hoverHeight;

            if ((isCheese && selectedCheeses.Count > 1) || (isSausage || isTomato))
            {
                Vector3 leaderNewPos = new Vector3(worldPoint.x, targetHeight, worldPoint.z);

                foreach (var item in selectedCheeses)
                {
                    Vector3 randOffset = randomOffsets.ContainsKey(item) ? randomOffsets[item] : Vector3.zero;
                    item.transform.position = new Vector3(
                        leaderNewPos.x + randOffset.x,
                        targetHeight,
                        leaderNewPos.z + randOffset.z
                    );

                    if (isCheese && randomRotations.ContainsKey(item))
                    {
                        float angle = randomRotations[item];
                        item.transform.rotation = Quaternion.Euler(-90f, angle, 0f);
                    }
                }
            }
            else
            {
                transform.position = new Vector3(worldPoint.x, targetHeight, worldPoint.z);
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Pizza"))
            {
                if ((isCheese && selectedCheeses.Count > 1) || (isSausage || isTomato))
                {
                    foreach (var item in selectedCheeses)
                    {
                        Vector3 offset = randomOffsets.ContainsKey(item) ? randomOffsets[item] : Vector3.zero;
                        AttachToPizza(item, hit, offset);

                        if (isCheese && randomRotations.ContainsKey(item))
                        {
                            float angle = randomRotations[item];
                            item.transform.rotation = Quaternion.Euler(-90f, angle, 0f);
                        }
                        GameEvents.CompleteStep(stepToUnlock);
                    }
                    ClearSelection();
                }
                else
                {
                    AttachToPizza(this, hit, Vector3.zero);
                    GameEvents.CompleteStep(stepToUnlock);
                }
            }
            else
            {
                ClearSelection();
            }
        }
        else
        {
            ClearSelection();
        }
    }

    private void AttachToPizza(DragFood food, RaycastHit hit, Vector3 offset)
    {
        Vector3 hitPoint = hit.point;

        Vector3 finalPosition = new Vector3(
            hitPoint.x + offset.x,
            hitPoint.y,
            hitPoint.z + offset.z
        );

        food.transform.SetParent(hit.collider.transform);

        food.transform.position = finalPosition;

        if (food.isCheese)
        {
            float angle = randomRotations.ContainsKey(food) ? randomRotations[food] : 0f;
            food.transform.rotation = Quaternion.Euler(-90f, angle, 0f);
        }
    }

    private void ClearSelection()
    {
        selectedCheeses.Clear();
        randomOffsets.Clear();
        randomRotations.Clear();
    }
}
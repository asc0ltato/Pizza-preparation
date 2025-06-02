using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShelfManager : MonoBehaviour
{
    public Transform[] slots;
    private List<Transform> occupiedSlots = new List<Transform>();
    private float moveDuration = 1.5f;

    public bool PlacePizzaInOrder(GameObject pizza)
    {
        foreach (Transform slot in slots)
        {
            if (!occupiedSlots.Contains(slot))
            {
                pizza.GetComponent<MonoBehaviour>().StartCoroutine(MovePizzaToSlot(pizza, slot));
                occupiedSlots.Add(slot);
                return true;
            }
        }

        Debug.Log("Все полки заняты");
        return false;
    }

    private IEnumerator MovePizzaToSlot(GameObject pizza, Transform slot)
    {
        Vector3 startPos = pizza.transform.position;
        Quaternion startRot = pizza.transform.rotation;
        Vector3 endPos = slot.position;
        Quaternion endRot = slot.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            pizza.transform.position = Vector3.Lerp(startPos, endPos, t);
            pizza.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pizza.transform.position = endPos;
        pizza.transform.rotation = endRot;
        pizza.transform.SetParent(slot);

        PizzaTimer.Instance?.StopTimer();
        PizzaStageManager.Instance.ClearCurrentPizzaReference();
        PizzaStageManager.Instance.SpawnNewPizza();
        RollingMove rollingMove = FindFirstObjectByType<RollingMove>();
        if (rollingMove != null)
        {
            rollingMove.RefreshPizzaReferences();
        }
        SaucePouring saucePouring = FindFirstObjectByType<SaucePouring>();
        if (saucePouring != null)
        {
            saucePouring.RefreshKetchupSpotReference();
        }
        UIStageButtons stageButtons = FindFirstObjectByType<UIStageButtons>();
        if (stageButtons != null)
        {
            stageButtons.ResetButtons();
        }
    }
}
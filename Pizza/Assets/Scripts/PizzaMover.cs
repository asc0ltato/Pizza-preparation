using UnityEngine;
using System.Collections;

public class PizzaMover : MonoBehaviour
{
    private Transform pizza => PizzaStageManager.Instance.GetCurrentPizzaTransform();
    public Transform targetPoint1;
    public Transform targetPoint2;
    public Transform targetPoint3;
    public Transform targetPoint4;
    public CameraSwitcher cameraSwitcher;
    public int stepToUnlock = 6;
    private bool isMoving = false;

    private void Update()
    {
        if (!IsOverheadCameraActive()) return;

        if (Input.GetKeyDown(KeyCode.Alpha6) && !isMoving && pizza != null && targetPoint1 != null && targetPoint2 != null && targetPoint3 != null && targetPoint4 != null)
        {
            StartCoroutine(MovePizza());
        }
    }
    private bool IsOverheadCameraActive()
    {
        return cameraSwitcher != null && cameraSwitcher.IsOverheadCameraActive();
    }

    private IEnumerator MovePizza()
    {
        yield return StartCoroutine(MovePizzaToTarget(targetPoint1, 2.0f));
        yield return StartCoroutine(MovePizzaToTarget(targetPoint2, 5.0f));
        yield return StartCoroutine(MovePizzaToTarget(targetPoint3, 1.0f));
        yield return StartCoroutine(MovePizzaToTarget(targetPoint4, 1.0f));
        GameEvents.CompleteStep(stepToUnlock);
    }

    private IEnumerator MovePizzaToTarget(Transform targetPoint, float duration)
    {
        isMoving = true;

        Vector3 startPos = pizza.position;
        Vector3 targetPos = targetPoint.position;

        Quaternion startRot = pizza.rotation;
        Vector3 targetEuler = targetPoint.eulerAngles;
        targetEuler.x = -90f;
        Quaternion targetRot = Quaternion.Euler(targetEuler);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            pizza.position = Vector3.Lerp(startPos, targetPos, t);
            pizza.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        pizza.position = targetPos;
        isMoving = false;
    }
}
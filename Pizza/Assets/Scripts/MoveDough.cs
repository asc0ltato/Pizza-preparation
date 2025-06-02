using UnityEngine;
using System.Collections;

public class MoveDough : MonoBehaviour
{
    private GameObject dough => PizzaStageManager.Instance.GetCurrentPizza();
    public Transform board;
    public CameraSwitcher cameraSwitcher;
    private float moveDuration = 1f;
    private float moveDuration2 = 2f;
    public int stepToUnlock = 1;
    private bool hasMoved = false;

    private void Start()
    {
        if (cameraSwitcher == null)
        {
            cameraSwitcher = FindAnyObjectByType<CameraSwitcher>();
        }
    }

    private void Update()
    {
        if (!IsOverheadCameraActive()) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) && !hasMoved)
        {
            StartCoroutine(MoveDoughSmoothly());
        }
    }

    private bool IsOverheadCameraActive()
    {
        return cameraSwitcher != null && cameraSwitcher.IsOverheadCameraActive();
    }

    private IEnumerator MoveDoughSmoothly()
    {
        PizzaTimer.Instance?.StartTimer();
        hasMoved = true;

        Vector3 startPosition = dough.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0.03095f, 0);

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            dough.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dough.transform.position = targetPosition;

        startPosition = dough.transform.position;
        targetPosition = new Vector3(board.position.x, startPosition.y, board.position.z);

        elapsedTime = 0f;
        while (elapsedTime < moveDuration2)
        {
            dough.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dough.transform.position = targetPosition;

        GameEvents.CompleteStep(stepToUnlock);
    }
}
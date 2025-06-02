using UnityEngine;
using System.Collections;

public class ClickButton : MonoBehaviour
{
    public Transform buttonTransform;
    public Conveyor conveyor;
    private Vector3 originalPosition;
    private float pressDuration = 0.2f;
    private bool isPressing = false;

    void Start()
    {
        if (buttonTransform == null)
        {
            buttonTransform = transform;
        }

        originalPosition = buttonTransform.localPosition;
    }

    void OnMouseDown()
    {
        if (!isPressing)
        {
            StartCoroutine(PressButton());
        }
    }

    IEnumerator PressButton()
    {
        isPressing = true;
        Vector3 targetPosition = originalPosition + new Vector3(0, -0.002f, -0.002f);

        float elapsed = 0f;
        while (elapsed < pressDuration)
        {
            buttonTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        buttonTransform.localPosition = targetPosition;

        if (conveyor != null)
        {
            conveyor.ToggleConveyor();
        }

        elapsed = 0f;
        while (elapsed < pressDuration)
        {
            buttonTransform.localPosition = Vector3.Lerp(targetPosition, originalPosition, elapsed / pressDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = originalPosition;
        isPressing = false;
    }
}
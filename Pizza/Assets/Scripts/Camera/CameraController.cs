using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [System.Serializable]
    public class CameraTarget
    {
        public Transform targetObject;
        public Vector3 cameraPosition;
        public Quaternion cameraRotation;
        public GameObject objectToHighlight;
        public string displayText;
    }

    public CameraTarget[] cameraTargets;
    public Transform playerCamera;
    private CameraTarget currentTarget;
    public TextChanger textChanger;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public Color highlightColor = Color.yellow;

    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    public float moveSpeed = 5f;
    private bool isZoomed = false;

    void Start()
    {
        originalPosition = playerCamera.position;
        originalRotation = playerCamera.rotation;
    }

    public void MoveCameraToObject(Transform obj)
    {
        CameraTarget target = FindCameraTargetByObject(obj);
        if (target == null)
        {
            Debug.LogWarning("Целевая камера не найденна на объект " + obj.name);
            return;
        }

        if (!isZoomed)
        {
            currentTarget = target;
            HighlightObject(true);

            if (textChanger != null)
                textChanger.ChangeText(currentTarget.displayText);

            StopAllCoroutines();
            StartCoroutine(MoveCamera(playerCamera.position, currentTarget.cameraPosition, playerCamera.rotation, currentTarget.cameraRotation));
            isZoomed = true;
        }
        else if (isZoomed && currentTarget.targetObject == obj)
        {
            HighlightObject(false);

            if (textChanger != null)
                textChanger.ChangeText("");

            StopAllCoroutines();
            StartCoroutine(MoveCamera(playerCamera.position, originalPosition, playerCamera.rotation, originalRotation));
            isZoomed = false;
            currentTarget = null;
        }
        else
        {
            HighlightObject(false);

            if (textChanger != null)
                textChanger.ChangeText("");

            StopAllCoroutines();
            StartCoroutine(SwitchTarget(target));
        }
    }

    CameraTarget FindCameraTargetByObject(Transform obj)
    {
        foreach (var ct in cameraTargets)
        {
            if (ct.targetObject == obj) return ct;
        }
        return null;
    }

    IEnumerator SwitchTarget(CameraTarget newTarget)
    {
        yield return MoveCamera(playerCamera.position, originalPosition, playerCamera.rotation, originalRotation);
        isZoomed = false;
        currentTarget = null;

        currentTarget = newTarget;
        HighlightObject(true);

        if (textChanger != null)
            textChanger.ChangeText(currentTarget.displayText);

        yield return MoveCamera(playerCamera.position, currentTarget.cameraPosition, playerCamera.rotation, currentTarget.cameraRotation);
        isZoomed = true;
    }

    IEnumerator MoveCamera(Vector3 fromPos, Vector3 toPos, Quaternion fromRot, Quaternion toRot)
    {
        float elapsed = 0f;
        float duration = 1f / moveSpeed;

        while (elapsed < duration)
        {
            playerCamera.position = Vector3.Lerp(fromPos, toPos, elapsed / duration);
            playerCamera.rotation = Quaternion.Slerp(fromRot, toRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.position = toPos;
        playerCamera.rotation = toRot;
    }

    void HighlightObject(bool highlight)
    {
        if (currentTarget == null || currentTarget.objectToHighlight == null) return;

        Renderer parentRenderer = currentTarget.objectToHighlight.GetComponent<Renderer>();
        if (parentRenderer != null)
        {
            ApplyHighlight(parentRenderer, highlight);
        }

        foreach (Renderer renderer in currentTarget.objectToHighlight.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name == "Жидкость")
                continue;

            ApplyHighlight(renderer, highlight);
        }
    }

    void ApplyHighlight(Renderer renderer, bool highlight)
    {
        if (highlight)
        {
            if (!originalColors.ContainsKey(renderer))
            {
                originalColors[renderer] = renderer.material.color;
            }
            renderer.material.color = highlightColor;
        }
        else
        {
            if (originalColors.ContainsKey(renderer))
            {
                renderer.material.color = originalColors[renderer];
                originalColors.Remove(renderer);
            }
        }
    }
}
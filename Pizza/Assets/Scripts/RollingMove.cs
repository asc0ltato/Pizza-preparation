using UnityEngine;
using System.Collections;

public class RollingMove : MonoBehaviour
{
    public GameObject rolling;
    private Transform dough => PizzaStageManager.Instance.GetCurrentPizzaTransform();
    public CameraSwitcher cameraSwitcher;
    public SkinnedMeshRenderer doughMesh;
    public SphereCollider sphereCollider; 
    public BoxCollider boxCollider;      
    Vector3 startPosition;
    Vector3 targetPosition;
    private Quaternion initialRotation;
    private float moveDuration = 1f;
    private float moveDuration2 = 2f;
    private int flatten30Index, flatten60Index, flatten90Index;
    public int stepToUnlock = 2;
    private enum RollingState { Idle, ReadyToRoll, RollingComplete, Finished }
    private RollingState currentState = RollingState.Idle;

    private void Start()
    {
        startPosition = rolling.transform.position;
        initialRotation = rolling.transform.rotation;

        if (cameraSwitcher == null)
        {
            cameraSwitcher = FindAnyObjectByType<CameraSwitcher>();
        }
    }

    private void Update()
    {
        if (!IsOverheadCameraActive()) return;

        if (Input.GetKeyDown(KeyCode.Alpha2) && currentState == RollingState.ReadyToRoll)
        {
            StartCoroutine(RolloutAnimation());
            GameEvents.CompleteStep(stepToUnlock);
        }
    }

    private bool IsOverheadCameraActive()
    {
        return cameraSwitcher != null && cameraSwitcher.IsOverheadCameraActive();
    }

    private void OnMouseDown()
    {
        if (!IsOverheadCameraActive()) return;

        GameObject pizza = PizzaStageManager.Instance.GetCurrentPizza();
        if (pizza == null)
        {
            Debug.LogWarning("Нет активной пиццы");
            return;
        }

        doughMesh = pizza.GetComponentInChildren<SkinnedMeshRenderer>();
        sphereCollider = pizza.GetComponentInChildren<SphereCollider>();
        boxCollider = pizza.GetComponentInChildren<BoxCollider>();

        if (doughMesh == null || sphereCollider == null || boxCollider == null)
        {
            Debug.LogWarning("Не найдены нужные компоненты у текущей пиццы");
            return;
        }

        flatten30Index = doughMesh.sharedMesh.GetBlendShapeIndex("30");
        flatten60Index = doughMesh.sharedMesh.GetBlendShapeIndex("60");
        flatten90Index = doughMesh.sharedMesh.GetBlendShapeIndex("90");

        switch (currentState)
        {
            case RollingState.Idle:
                StartCoroutine(MoveToRollingPosition());
                break;

            case RollingState.RollingComplete:
                StartCoroutine(ReturnRollingPinToStartPosition());
                break;

            case RollingState.Finished:
                break;
        }
    }
    private IEnumerator MoveToRollingPosition()
    {
        currentState = RollingState.ReadyToRoll;

        yield return StartCoroutine(RotateAndMoveRollingPin());
        yield return StartCoroutine(MoveRollingPin());
    }

    private IEnumerator RotateAndMoveRollingPin()
    {
        Quaternion startRotation = rolling.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);

        targetPosition = startPosition + new Vector3(0, 0.06f, 0);

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            rolling.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / moveDuration);
            rolling.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rolling.transform.rotation = targetRotation;
        rolling.transform.position = targetPosition;
    }

    private IEnumerator MoveRollingPin()
    {
        Vector3 startPosition = rolling.transform.position;
        Vector3 targetPosition = dough.position + new Vector3(-0.028f, 0.06f, 0.03f);

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration2)
        {
            rolling.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rolling.transform.position = targetPosition;
    }

    private IEnumerator RolloutAnimation()
    {
        float duration = 2f;
        float elapsedTime = 0f;

        float doughStartHeight = 0.326201f;
        float doughEndHeight = 0.28357f;

        Vector3 doughInitialPosition = dough.position;
        Vector3 rollingInitialPosition = rolling.transform.position;

        float rollingZMovementRange = 0.07f;
        float rollingDownwardPressure = -0.0003f;
        int rollCycles = 3;

        float rollingStartDelayPercent = 0.1f;

        float rollingZStartOffset = 0.008f;
        rollingInitialPosition.z = dough.position.z + rollingZStartOffset;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            float blend30 = 0f, blend60 = 0f, blend90 = 0f;

            if (progress < 0.25f)
                blend30 = Mathf.Lerp(0f, 100f, progress / 0.25f);
            else if (progress < 0.5f)
                blend30 = Mathf.Lerp(100f, 0f, (progress - 0.25f) / 0.25f);

            if (progress >= 0.25f && progress < 0.5f)
                blend60 = Mathf.Lerp(0f, 100f, (progress - 0.25f) / 0.25f);
            else if (progress >= 0.5f && progress < 0.75f)
                blend60 = Mathf.Lerp(100f, 50f, (progress - 0.5f) / 0.25f);
            else if (progress >= 0.75f)
                blend60 = Mathf.Lerp(50f, 0f, (progress - 0.75f) / 0.25f);

            if (progress >= 0.5f && progress < 0.75f)
                blend90 = Mathf.Lerp(0f, 50f, (progress - 0.5f) / 0.25f);
            else if (progress >= 0.75f)
                blend90 = Mathf.Lerp(50f, 100f, (progress - 0.75f) / 0.25f);

            SetWeights(blend30, blend60, blend90);

            float currentDoughY = Mathf.Lerp(doughStartHeight, doughEndHeight, progress);
            dough.position = new Vector3(doughInitialPosition.x, currentDoughY, doughInitialPosition.z);

            float doughTopY = currentDoughY + 0.02f;

            float rollingY;
            if (progress < rollingStartDelayPercent)
            {
                rollingY = doughTopY + 0.03f;
            }
            else
            {
                float pressureT = (progress - rollingStartDelayPercent) / (1f - rollingStartDelayPercent);
                float pressureDepth = Mathf.Lerp(0f, rollingDownwardPressure, pressureT);
                rollingY = doughTopY + pressureDepth;
            }

            rollingY = Mathf.Max(rollingY, doughTopY + 0.02f);

            if (rollingY < doughTopY)
            {
                rollingY = doughTopY + 0.0015f;
            }

            float rollingZ = dough.position.z + rollingZStartOffset;

            if (progress >= rollingStartDelayPercent)
            {
                float tZ = (progress - rollingStartDelayPercent) / (1f - rollingStartDelayPercent);
                float phase = tZ * rollCycles;
                float zOffset = Mathf.PingPong(phase, 1f) * rollingZMovementRange - rollingZMovementRange / 2f;
                rollingZ = dough.position.z + rollingZStartOffset + zOffset;
            }

            rolling.transform.position = new Vector3(
                rollingInitialPosition.x,
                rollingY,
                rollingZ
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetWeights(0f, 0f, 100f);
        dough.position = new Vector3(doughInitialPosition.x, doughEndHeight, doughInitialPosition.z);

        if (sphereCollider != null && boxCollider != null)
        {
            sphereCollider.enabled = false;
            boxCollider.enabled = true;
            Debug.Log("Смена коллайдеров");
        }

        currentState = RollingState.RollingComplete;
    }

    private void SetWeights(float blend30, float blend60, float blend90)
    {
        doughMesh.SetBlendShapeWeight(flatten30Index, blend30);
        doughMesh.SetBlendShapeWeight(flatten60Index, blend60);
        doughMesh.SetBlendShapeWeight(flatten90Index, blend90);
    }

    private IEnumerator ReturnRollingPinToStartPosition()
    {
        Vector3 initialPosition = startPosition;
        Vector3 currentPosition = rolling.transform.position;
        Quaternion currentRotation = rolling.transform.rotation;

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 targetPosition = new Vector3(
                Mathf.Lerp(currentPosition.x, initialPosition.x, elapsedTime / duration),
                Mathf.Lerp(currentPosition.y, initialPosition.y, elapsedTime / duration),
                startPosition.z
            );

            rolling.transform.position = targetPosition;
            rolling.transform.rotation = Quaternion.Slerp(currentRotation, initialRotation, elapsedTime / duration); 

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rolling.transform.position = initialPosition;
        rolling.transform.rotation = initialRotation;

        currentState = RollingState.Finished;
    }

    public void RefreshPizzaReferences()
    {
        GameObject pizza = PizzaStageManager.Instance.GetCurrentPizza();
        if (pizza != null)
        {
            doughMesh = pizza.GetComponentInChildren<SkinnedMeshRenderer>();
            sphereCollider = pizza.GetComponentInChildren<SphereCollider>();
            boxCollider = pizza.GetComponentInChildren<BoxCollider>();

            flatten30Index = doughMesh.sharedMesh.GetBlendShapeIndex("30");
            flatten60Index = doughMesh.sharedMesh.GetBlendShapeIndex("60");
            flatten90Index = doughMesh.sharedMesh.GetBlendShapeIndex("90");

            currentState = RollingState.Idle;
            Debug.Log("Ссылки на пиццу обновлены");
        }
        else
        {
            Debug.LogWarning("Пицца не найдена при обновлении ссылок");
        }
    }

}
using System.Collections;
using UnityEngine;

public class SaucePouring : MonoBehaviour
{
    public Transform scoop;
    public Transform container;
    private Transform pizza => PizzaStageManager.Instance.GetCurrentPizzaTransform();
    public Transform liquidInContainer;
    public Transform liquidInBowl;
    public Transform bowl;
    private SphereCollider scoopCollider;
    public ParticleSystem sauceStream;
    public CameraSwitcher cameraSwitcher;
    public Transform ketchupSpot;

    private Vector3 liquidBaseSize;
    private Vector3 scoopInitialPosition;
    private Quaternion scoopInitialRotation;

    private float currentLiquidInContainer;
    private float currentKetchupRadius = 0f;
    private float maxKetchupRadius = 0.04f; 
    private float ketchupGrowthSpeed = 1.0f; 
    private float maxLiquidLevel = 5.0f;
    private float scoopCapacity = 0.9f;
    private float currentLiquidInScoop = 0f;
    private float transferSpeed = 0.5f;
    private float pourHeight = 0.1f;
    public int stepToUnlock = 3;

    private enum ScoopState { Idle, Capturing, MovingToPizza, Pouring, Returning }
    private ScoopState scoopState = ScoopState.Idle;

    void Start()
    {
        currentLiquidInContainer = maxLiquidLevel;
        scoopInitialPosition = scoop.position;
        scoopInitialRotation = scoop.rotation;

        if (cameraSwitcher == null)
        {
            cameraSwitcher = FindAnyObjectByType<CameraSwitcher>();
        }

        scoopCollider = bowl.GetComponent<SphereCollider>();
        if (scoopCollider == null)
        {
            Debug.LogError("SphereCollider отсутствует на объекте bowl");
        }

        Renderer liquidRenderer = liquidInBowl.GetComponentInChildren<Renderer>();
        if (liquidRenderer != null)
        {
            Vector3 worldSize = liquidRenderer.bounds.size;
            liquidBaseSize = liquidInBowl.InverseTransformVector(worldSize);
        }
        else
        {
            Debug.LogWarning("Не найден Renderer у liquidInBowl");
            liquidBaseSize = new Vector3(0.1f, 0.1f, 0.1f);
        }

        RefreshKetchupSpotReference();
    }

    void Update()
    {
        if (!IsOverheadCameraActive()) return;

        if (Input.GetKeyDown(KeyCode.Alpha3) && scoopState == ScoopState.Idle)
        {
            StartCoroutine(CaptureAndMoveToPizza());
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && currentLiquidInScoop > 0 && scoopState == ScoopState.Idle)
        {
            StartCoroutine(PourSauce());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && scoopState == ScoopState.Idle)
        {
            StartCoroutine(ReturnScoop());
            GameEvents.CompleteStep(stepToUnlock);
        }
    }

    private bool IsOverheadCameraActive()
    {
        return cameraSwitcher != null && cameraSwitcher.IsOverheadCameraActive();
    }

    private IEnumerator CaptureAndMoveToPizza()
    {
        scoopState = ScoopState.Capturing;
        yield return StartCoroutine(CaptureScoop());
        scoopState = ScoopState.MovingToPizza;
        yield return StartCoroutine(MoveScoopToPizza());
        scoopState = ScoopState.Idle;
    }

    private IEnumerator ReturnScoop()
    {
        scoopState = ScoopState.Returning;
        yield return StartCoroutine(ReturnScoopToContainer());
        yield return StartCoroutine(ReturnScoopInContainer());
        scoopState = ScoopState.Idle;
    }

    IEnumerator CaptureScoop()
    {
        scoopState = ScoopState.Capturing;

        Vector3 startPos = scoop.position;
        Vector3 targetPos = startPos + new Vector3(0, 0.1f, 0);
        Quaternion startRot = scoop.rotation;
        Quaternion targetRot = Quaternion.Euler(-90, 0, 0);

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            scoop.position = Vector3.Lerp(startPos, targetPos, progress);
            scoop.rotation = Quaternion.Slerp(startRot, targetRot, progress);

            float amountToTransfer = transferSpeed * Time.deltaTime;
            amountToTransfer = Mathf.Min(amountToTransfer, currentLiquidInContainer);
            amountToTransfer = Mathf.Min(amountToTransfer, scoopCapacity - currentLiquidInScoop);

            currentLiquidInContainer -= amountToTransfer;
            currentLiquidInScoop += amountToTransfer;

            UpdateLiquidVisuals();

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoop.position = targetPos;
        scoop.rotation = targetRot;

        scoopState = ScoopState.Idle;
    }

    IEnumerator MoveScoopToPizza()
    {
        scoopState = ScoopState.MovingToPizza;

        Vector3 startPos = scoop.position;
        Vector3 targetPos = pizza.position + Vector3.up * pourHeight;

        Quaternion startRot = scoop.rotation;
        Quaternion targetRot = Quaternion.Euler(-90, 0, 0);

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            scoop.position = Vector3.Lerp(startPos, targetPos, t);
            scoop.rotation = Quaternion.Slerp(startRot, targetRot, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoop.position = targetPos;
        scoop.rotation = targetRot;

        scoopState = ScoopState.Idle;
    }

    void UpdateLiquidVisuals()
    {
        float containerFillPercent = Mathf.Clamp01(currentLiquidInContainer / maxLiquidLevel);
        Vector3 containerScale = liquidInContainer.localScale;
        liquidInContainer.localScale = new Vector3(containerScale.x, containerFillPercent, containerScale.z);

        float scoopFillPercent = Mathf.Clamp01(currentLiquidInScoop / scoopCapacity);

        if (scoopCollider != null && liquidBaseSize != Vector3.zero)
        {
            float scoopRadius = scoopCollider.radius;
            float baseRadius = liquidBaseSize.x / 2f;

            float visualScaleFactor = 3f;
            float maxScale = (scoopRadius / baseRadius) * visualScaleFactor;
            float visualScale = scoopFillPercent * maxScale;

            liquidInBowl.localScale = new Vector3(visualScale, visualScale, visualScale);
            float liftOffset = baseRadius * visualScale;
            liquidInBowl.localPosition = new Vector3(-0.08f, 0.03f, 0);
        }
    }

    IEnumerator PourSauce()
    {
        scoopState = ScoopState.Pouring;

        if (sauceStream != null)
            sauceStream.Play();

        if (ketchupSpot != null)
        {
            Transform currentPizza = PizzaStageManager.Instance.GetCurrentPizzaTransform();
            if (currentPizza != null)
            {
                ketchupSpot.SetParent(currentPizza);
            }

            ketchupSpot.gameObject.SetActive(true);
            currentKetchupRadius = 0f;
            ketchupSpot.localScale = Vector3.zero;
        }

        float duraction = 1.5f;
        float elapsedTime = 0f;

        Quaternion startRot = scoop.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);

        while (elapsedTime < duraction && currentLiquidInScoop > 0)
        {
            float progress = elapsedTime / duraction;
            scoop.rotation = Quaternion.Slerp(startRot, targetRot, progress);

            float amountToPour = transferSpeed * Time.deltaTime;

            if (currentLiquidInScoop <= 0f)
            {
                currentLiquidInScoop = 0f;
                break;
            }

            amountToPour = Mathf.Min(amountToPour, currentLiquidInScoop);
            currentLiquidInScoop -= amountToPour;

            UpdateLiquidVisuals();

            if (ketchupSpot != null)
            {
                float targetSpotRadius = Mathf.Lerp(0f, maxKetchupRadius, (scoopCapacity - currentLiquidInScoop) / scoopCapacity);
                currentKetchupRadius = Mathf.Lerp(currentKetchupRadius, targetSpotRadius, Time.deltaTime * ketchupGrowthSpeed);
                float ovalFactor = 1.2f;
                ketchupSpot.localScale = new Vector3(currentKetchupRadius * ovalFactor, currentKetchupRadius, 1f);
                ketchupSpot.position = pizza.position + Vector3.up * 0.01f;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (sauceStream != null)
        {
            sauceStream.Stop();
        }    

        float returnDuration = 1f;
        float returnElapsedTime = 0f;
        Quaternion pourEndRot = scoop.rotation;

        while (returnElapsedTime < returnDuration)
        {
            float t = returnElapsedTime / returnDuration;
            scoop.rotation = Quaternion.Slerp(pourEndRot, startRot, t);
            returnElapsedTime += Time.deltaTime;
            yield return null;
        }

        scoop.rotation = startRot;

        scoopState = ScoopState.Idle;
    }

    IEnumerator ReturnScoopToContainer()
    {
        Vector3 startPos = scoop.position;
        Vector3 targetPos = scoopInitialPosition + new Vector3(0, 0.1f, 0);

        Quaternion startRot = scoop.rotation;
        Quaternion targetRot = scoopInitialRotation;

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            scoop.position = Vector3.Lerp(startPos, targetPos, progress);
            scoop.rotation = Quaternion.Slerp(startRot, targetRot, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoop.position = targetPos;
        scoop.rotation = targetRot;
    }

    IEnumerator ReturnScoopInContainer()
    {
        Vector3 startPos = scoop.position;
        Vector3 targetPos = scoopInitialPosition;

        Quaternion startRot = scoop.rotation;
        Quaternion targetRot = scoopInitialRotation;

        float duration = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            scoop.position = Vector3.Lerp(startPos, targetPos, progress);
            scoop.rotation = Quaternion.Slerp(startRot, targetRot, progress);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scoop.position = targetPos;
        scoop.rotation = targetRot;
    }

    public void RefreshKetchupSpotReference()
    {
        if (pizza != null)
        {
            ketchupSpot = pizza.Find("KetchupCircle");
            if (ketchupSpot != null)
            {
                ketchupSpot.gameObject.SetActive(false);
                ketchupSpot.localScale = Vector3.zero;
            }
        }
        else
        {
            ketchupSpot = null;
        }
    }
}
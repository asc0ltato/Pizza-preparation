using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private Renderer rend;
    private Vector2 offset;
    private float scrollSpeed = 3f;
    public int stepToUnlock = 5;
    private bool isRunning = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = Vector2.zero;
    }

    void Update()
    {
        if (isRunning)
        {
            offset.x += scrollSpeed * Time.deltaTime;
            rend.material.SetTextureOffset("_BaseMap", offset);
        }
    }

    public void ToggleConveyor()
    {
        isRunning = !isRunning;
        GameEvents.CompleteStep(stepToUnlock);
    }
}
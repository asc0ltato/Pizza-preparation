using TMPro;
using UnityEngine;

public class PizzaTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public static PizzaTimer Instance;
    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        timerText.text = FormatTime(elapsedTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void StartTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        if (timerText != null)
            timerText.text = "";
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
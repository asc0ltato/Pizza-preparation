using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PizzaInputManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text[] sausageTexts;
    public TMP_Text[] tomatoTexts;
    public TMP_Text[] cheeseTexts;
    public TMP_Text[] timeTexts;
    public TMP_Text averageSausageText;
    public TMP_Text averageTomatoText;
    public TMP_Text averageCheeseText;
    public TMP_Text averageTimeText;

    private List<int> sausageData = new List<int>();
    private List<int> tomatoData = new List<int>();
    private List<int> cheeseData = new List<int>();
    private List<float> timeData = new List<float>();

    private int currentAttempt = 0;
    private int currentParameter = 0;
    private const int maxAttempts = 3;
    private const int totalParameters = 4;

    public void OnSubmit()
    {
        if (currentAttempt >= maxAttempts)
        {
            inputField.interactable = false;
            return;
        }

        if (string.IsNullOrEmpty(inputField.text))
        {
            return;
        }

        bool parseSuccess = false;
        if (currentParameter == 0 || currentParameter == 1 || currentParameter == 2)
        {
            int val;
            parseSuccess = int.TryParse(inputField.text, out val);
            if (!parseSuccess || val < 0)
            {
                return;
            }

            if (currentParameter == 0)
            {
                sausageData.Add(val);
                sausageTexts[currentAttempt].text = val.ToString();
            }
            else if (currentParameter == 1)
            {
                tomatoData.Add(val);
                tomatoTexts[currentAttempt].text = val.ToString();
            }
            else if (currentParameter == 2) {
                cheeseData.Add(val);
                cheeseTexts[currentAttempt].text = val.ToString();
            }
        }
        else if (currentParameter == 3)
        {
            string input = inputField.text.Trim();
            float val = 0f;

            string[] parts = input.Split(',');

            if (parts.Length == 2)
            {
                bool parseMinutes = int.TryParse(parts[0], out int minutes);
                bool parseSeconds = int.TryParse(parts[1], out int seconds);

                if (parseMinutes && parseSeconds && seconds >= 0 && seconds < 60)
                {
                    val = minutes * 60 + seconds;
                    parseSuccess = true;
                }
            }
            else
            {
                parseSuccess = float.TryParse(input, out val);
            }

            if (parseSuccess && val >= 0f)
            {
                timeData.Add(val);
                timeTexts[currentAttempt].text = FormatTime(val);
            }
        }

        inputField.text = "";
        currentParameter++;

        if (currentParameter >= totalParameters)
        {
            currentParameter = 0;
            currentAttempt++;

            if (currentAttempt >= maxAttempts)
            {
                CalculateAndDisplayAverages();
                inputField.interactable = false;
                return;
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes}:{seconds:00}";
    }

    void CalculateAndDisplayAverages()
    {
        averageSausageText.text = $"{sausageData.Average():F1}";
        averageTomatoText.text = $"{tomatoData.Average():F1}";
        averageCheeseText.text = $"{cheeseData.Average():F1}";
        float averageTime = timeData.Average();
        averageTimeText.text = FormatTime(averageTime);
    }

    public void RemoveLastInput()
    {
        if (!inputField.interactable)
        {
            return;
        }

        if (currentAttempt == 0 && currentParameter == 0)
        {
            return;
        }

        currentParameter--;
        if (currentParameter < 0)
        {
            currentParameter = totalParameters - 1;
            currentAttempt--;
            if (currentAttempt < 0)
            {
                currentAttempt = 0;
                currentParameter = 0;
                return;
            }
        }

        switch (currentParameter)
        {
            case 0:
                if (sausageData.Count > 0)
                {
                    sausageData.RemoveAt(sausageData.Count - 1);
                    sausageTexts[currentAttempt].text = "-";
                }
                break;
            case 1:
                if (tomatoData.Count > 0)
                {
                    tomatoData.RemoveAt(tomatoData.Count - 1);
                    tomatoTexts[currentAttempt].text = "-";
                }
                break;
            case 2:
                if (cheeseData.Count > 0)
                {
                    cheeseData.RemoveAt(cheeseData.Count - 1);
                    cheeseTexts[currentAttempt].text = "-";
                }
                break;
            case 3:
                if (timeData.Count > 0)
                {
                    timeData.RemoveAt(timeData.Count - 1);
                    timeTexts[currentAttempt].text = "-";
                }
                break;
        }
    }
}
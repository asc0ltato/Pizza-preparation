using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour
{
    public TMP_Text targetText;

    public void ChangeText(string newText)
    {
        targetText.text = newText;
    }
}
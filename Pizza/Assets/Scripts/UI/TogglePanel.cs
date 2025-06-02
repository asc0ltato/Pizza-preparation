using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject[] panels;

    void Start()
    {
        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }

    public void Toggle()
    {
        foreach (var panel in panels)
        {
            if (panel != null)
                panel.SetActive(!panel.activeSelf);
        }
    }
}
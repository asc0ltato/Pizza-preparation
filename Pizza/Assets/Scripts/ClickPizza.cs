using UnityEngine;

public class ClickPizza : MonoBehaviour
{
    public ShelfManager shelfManager;
    private bool hasMoved = false;
    public UIStageButtons uiStageButtons;

    private void OnMouseDown()
    {
        if (uiStageButtons == null || !uiStageButtons.stageButtons[6].interactable)
            return;

        if (hasMoved) return;

        bool placed = shelfManager.PlacePizzaInOrder(gameObject);
        if (placed)
        {
            hasMoved = true;
        }
    }
}
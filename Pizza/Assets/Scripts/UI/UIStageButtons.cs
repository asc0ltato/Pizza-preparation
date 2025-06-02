using UnityEngine;
using UnityEngine.UI;

public class UIStageButtons : MonoBehaviour
{
    public Button[] stageButtons;

    private void Start()
    {
        for (int i = 1; i < stageButtons.Length; i++)
        {
            stageButtons[i].interactable = false;
        }

        GameEvents.OnStepCompleted.AddListener(UnlockStageButton);
    }

    private void UnlockStageButton(int completedStepIndex)
    {
        int nextButtonIndex = completedStepIndex;
        if (nextButtonIndex < stageButtons.Length && !stageButtons[nextButtonIndex].interactable)
        {
            stageButtons[nextButtonIndex].interactable = true;
            Debug.Log($"������ ����� {nextButtonIndex + 1} ��������������");
        }
    }

    public void ResetButtons()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            stageButtons[i].interactable = (i == 0);
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnStepCompleted.RemoveListener(UnlockStageButton);
    }
}
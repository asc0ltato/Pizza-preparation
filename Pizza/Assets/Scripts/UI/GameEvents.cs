using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static UnityEvent<int> OnStepCompleted = new UnityEvent<int>();

    public static void CompleteStep(int stepIndex)
    {
        OnStepCompleted.Invoke(stepIndex);
    }
}
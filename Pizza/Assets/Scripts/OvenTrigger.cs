using UnityEngine;

public class OvenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            PizzaBaking baking = other.GetComponent<PizzaBaking>();
            if (baking != null)
            {
                baking.StartBaking();
            }
        }
    }
}
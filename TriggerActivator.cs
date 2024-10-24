using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    // Reference to the GameObject you want to enable
    public GameObject objectToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateFunction();
        }
    }

    private void ActivateFunction()
    {
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false); // Enable the GameObject
            Debug.Log($"{objectToEnable.name} has been activated!");
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to enable.");
        }
    }
}

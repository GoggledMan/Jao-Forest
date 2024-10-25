using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    // Reference to the GameObject you want to enable (initially hidden)
    public GameObject objectToEnable;

    // Reference to the text box you want to hide
    public GameObject textBoxToHide;

    // Reference to the text box you want to show
    public GameObject textBoxToShow;

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
            objectToEnable.SetActive(false); // Hide the GameObject
            Debug.Log($"{objectToEnable.name} has been deactivated!");

            HideAndShowTextBoxes();
        }
        else
        {
            Debug.LogWarning("No GameObject assigned to enable.");
        }
    }

    private void HideAndShowTextBoxes()
    {
        if (textBoxToHide != null)
        {
            textBoxToHide.SetActive(false); // Hide the text box
            Debug.Log($"{textBoxToHide.name} has been hidden.");
        }
        else
        {
            Debug.LogWarning("No text box assigned to hide.");
        }

        if (textBoxToShow != null)
        {
            textBoxToShow.SetActive(true); // Show the text box
            Debug.Log($"{textBoxToShow.name} has been shown.");
        }
        else
        {
            Debug.LogWarning("No text box assigned to show.");
        }
    }
}

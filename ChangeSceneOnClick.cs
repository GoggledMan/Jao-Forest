using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneOnClick : MonoBehaviour
{
    public string sceneName; // The name of the scene to load

    void Start()
    {
        // Get the Button component attached to the same GameObject
        Button button = GetComponent<Button>();

        // Add a listener to the button click event
        button.onClick.AddListener(ChangeScene);
    }

    // Method to change the scene
    void ChangeScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
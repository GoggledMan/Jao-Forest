using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        #if UNITY_EDITOR
        // If we are in the editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If we are running the game
        Application.Quit();
        #endif
    }
}

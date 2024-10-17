using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign in the Inspector
    public string gameplaySceneName; // Name of the gameplay scene

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>(); // Automatically get the VideoPlayer component if not assigned
        }

        PlayCutscene();
    }

    void PlayCutscene()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play(); // Start playing the cutscene
            videoPlayer.loopPointReached += OnCutsceneFinished; // Register the event for when the cutscene ends
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned or found on this GameObject.");
        }
    }

    void OnCutsceneFinished(VideoPlayer vp)
    {
        LoadGameplayScene(); // Call method to load the gameplay scene when the cutscene ends
    }

    void LoadGameplayScene()
    {
        SceneManager.LoadScene(gameplaySceneName); // Load the gameplay scene
    }
}

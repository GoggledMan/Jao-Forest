using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // Reference to the VideoPlayer
    public RawImage videoDisplay;     // Reference to the RawImage
    public GameObject videoPanel;     // Optional: a panel to contain the video display

    private void Start()
    {
        // Initially disable the RawImage and the video panel
        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(false);
            Debug.Log("RawImage disabled.");
        }
        
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
            Debug.Log("Video panel disabled.");
        }

        // Ensure the VideoPlayer is not playing initially
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // Stop any video if playing
            Debug.Log("VideoPlayer stopped.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger.");

            if (videoPlayer != null && videoDisplay != null)
            {
                // Enable RawImage and the video panel
                videoDisplay.gameObject.SetActive(true);
                videoPanel.SetActive(true);
                Debug.Log("RawImage and video panel enabled.");

                // Create and set RenderTexture
                var renderTexture = RenderTexture.GetTemporary(1920, 1080); // Set desired resolution
                videoPlayer.targetTexture = renderTexture;
                videoDisplay.texture = renderTexture; // Set RawImage to use the RenderTexture

                // Play the video
                videoPlayer.Play();
                Debug.Log("Video started playing.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited trigger.");

            if (videoPlayer != null)
            {
                videoPlayer.Stop(); // Stop the video
                videoDisplay.gameObject.SetActive(false); // Disable RawImage
                videoPanel.SetActive(false);               // Disable the video panel

                // Release the RenderTexture if used
                if (videoPlayer.targetTexture != null)
                {
                    RenderTexture.ReleaseTemporary(videoPlayer.targetTexture);
                    videoPlayer.targetTexture = null;
                    Debug.Log("RenderTexture released.");
                }
            }
        }
    }
}

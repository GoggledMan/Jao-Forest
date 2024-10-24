using UnityEngine;
using UnityEngine.Video;

public class PlayVideoOnTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            videoPlayer.Play();
        }
    }
}
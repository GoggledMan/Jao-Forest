using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FootstepLayer
{
    public int layer; // Layer number (0 = Default, 8 = Wood, etc.)
    public AudioClip[] footstepSounds; // Footstep sounds for the layer
}

public class FootstepAudio : MonoBehaviour
{
    public float footstepInterval = 0.5f; // Time interval between footstep sounds
    private AudioSource audioSource;       // Reference to the AudioSource component
    private Coroutine footstepCoroutine;   // Reference to the coroutine

    public List<FootstepLayer> footstepLayers; // List of footstep layers

    private void Start()
    {
        // Add an AudioSource component if it doesn't exist
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if the player is moving in any direction
        float horizontalMovement = Input.GetAxis("Horizontal"); // A/D or Left/Right keys
        float verticalMovement = Input.GetAxis("Vertical"); // W/S or Up/Down keys

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            // Start the footstep sound coroutine if it's not already running
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(PlayFootsteps());
            }
        }
        else
        {
            // Stop the coroutine if the player stops moving
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null; // Reset coroutine reference
            }
        }
    }

    private IEnumerator PlayFootsteps()
    {
        while (true)
        {
            PlayRandomFootstepSound();
            yield return new WaitForSeconds(footstepInterval); // Wait for the specified interval
        }
    }

    private void PlayRandomFootstepSound()
    {
        AudioClip[] currentFootsteps = GetFootstepSoundsBasedOnLayer();

        // Randomly select a footstep sound from the current array
        if (currentFootsteps.Length > 0)
        {
            int randomIndex = Random.Range(0, currentFootsteps.Length);
            audioSource.clip = currentFootsteps[randomIndex]; // Assign the random clip
            audioSource.PlayOneShot(audioSource.clip); // Play the sound
        }
    }

    private AudioClip[] GetFootstepSoundsBasedOnLayer()
    {
        // Check what layer the player is currently standing on
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) // Raycast down to check the ground layer
        {
            int groundLayer = hit.collider.gameObject.layer; // Get the layer of the object the player is standing on

            // Debug log to check the layer being used
            Debug.Log($"Standing on Layer: {groundLayer} - Layer Name: {LayerMask.LayerToName(groundLayer)}");

            // Find the corresponding footstep sounds for the current ground layer
            foreach (var footstepLayer in footstepLayers)
            {
                if (footstepLayer.layer == groundLayer)
                {
                    return footstepLayer.footstepSounds; // Return the sounds for this layer
                }
            }
        }

        // If no ground layer sounds are found, fallback to a default (like grass)
        foreach (var footstepLayer in footstepLayers)
        {
            if (footstepLayer.layer == 0) // Assuming layer 0 is grass
            {
                return footstepLayer.footstepSounds; // Return the default sounds (e.g., grass)
            }
        }

        // Return an empty array if no sounds are found at all
        return new AudioClip[0];
    }
}
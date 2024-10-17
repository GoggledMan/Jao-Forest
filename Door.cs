using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioClip hitSound; // Sound to play when the door is hit
    public AudioClip breakSound; // Sound to play when the door breaks
    private AudioSource audioSource; // Reference to the AudioSource component
    public int health = 10; // Health of the door
    private MeshRenderer meshRenderer; // Reference to the MeshRenderer component

    public GameObject brokenDoorPrefab; // Reference to the broken door prefab

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        meshRenderer = GetComponent<MeshRenderer>(); // Get the MeshRenderer component

        // Ensure the broken door prefab is initially inactive
        if (brokenDoorPrefab != null)
        {
            brokenDoorPrefab.SetActive(false); // Deactivate it initially
        }
    }

    public void BreakDoor(int damage)
    {
        health -= damage; // Reduce health by damage amount
        Debug.Log("Door health: " + health); // Log current health

        // Play the hit sound every time the door is hit
        if (hitSound && audioSource)
        {
            audioSource.PlayOneShot(hitSound); // Play the hit sound
        }

        // Check if the door is broken
        if (health <= 0)
        {
            Debug.Log("Breaking door..."); // Log when starting to break the door

            // Play the break sound
            if (breakSound && audioSource)
            {
                audioSource.PlayOneShot(breakSound); // Play the break sound
            }

            // Hide the mesh renderer
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false; // Hide the door's mesh
                Debug.Log("Door mesh hidden."); // Log hiding action
            }

            // Disable all colliders
            Collider[] colliders = GetComponents<Collider>(); // Get all colliders attached to the door
            foreach (Collider col in colliders)
            {
                col.enabled = false; // Disable each collider
                Debug.Log(col.isTrigger ? "Trigger collider disabled." : "Door collider disabled."); // Log disabling action
            }

            // Activate the broken door prefab
            if (brokenDoorPrefab != null)
            {
                brokenDoorPrefab.SetActive(true); // Activate the broken door prefab
                Debug.Log("Broken door prefab activated."); // Log activation action
            }
        }
    }
}

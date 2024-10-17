using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai; // Controls the enemy's movement
    public float walkSpeed; // Enemy's walking speed
    public float chaseSpeed; // Enemy's chasing speed
    public float sightDistance; // Distance at which the enemy can see the player
    public float catchDistance; // Distance at which the enemy catches the player
    public Transform player; // Reference to the player
    public float searchRadius; // Radius around the player where the AI knows the player is located
    public float radiusEntryDistance = 1f; // Distance to determine if AI has entered the search radius
    public float idleTime, minIdleTime, maxIdleTime; // Idle timing variables
    bool inSearchRadius; // Checks if the AI is within the search radius
    bool chasing; // Checks if the enemy is chasing the player
    Vector3 lastKnownPosition; // Last known position of the player
    Coroutine idleCoroutine; // Manages the idle coroutine

    // Jumpscare components
    public Image jumpScareImage; // Reference to the image to show when the player is caught
    public float jumpScareDuration = 2f; // Duration for which the jumpscare image is shown

    // Audio components
    public AudioClip chaseAudio; // Audio clip for chasing sound
    private AudioSource audioSource; // Audio source to play the sound

    void Start()
    {
        ai.speed = walkSpeed; // Set the initial walking speed
        lastKnownPosition = Vector3.zero; // Initialize last known position

        // Hide the jump scare image at the start
        jumpScareImage.gameObject.SetActive(false);

        // Set up the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = chaseAudio; // Assign the chase audio clip
        audioSource.loop = true; // Enable looping for continuous play
    }

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float playerDistance = directionToPlayer.magnitude;

        // Update the last known position to the player's current position
        lastKnownPosition = player.position;

        // Rotate towards the player
        FacePlayer();

        // Stage 1: Move towards the player’s search radius
        if (!inSearchRadius)
        {
            if (playerDistance <= searchRadius)
            {
                inSearchRadius = true; // Entered the search radius
            }
            else
            {
                ai.destination = lastKnownPosition; // Move towards the player's general location
                ai.speed = walkSpeed; 
            }
        }
        // Stage 2: Search within the radius
        else if (inSearchRadius && !chasing)
        {
            ai.destination = GetRandomPointInRadius(lastKnownPosition, searchRadius); // Wander within the search radius
            ai.speed = walkSpeed;

            // Check for sighting of the player
            if (playerDistance < sightDistance)
            {
                StartChasingPlayer(); // Switch to chasing state
            }
        }

        // Stage 3: Chasing the player
        if (chasing)
        {
            ai.destination = player.position; // Set destination to player's position
            ai.speed = chaseSpeed; // Use chase speed

            if (Vector3.Distance(player.position, ai.transform.position) <= catchDistance)
            {
                PlayerCaught(); // Call the method for when the player is caught
            }
        }
    }

    // Rotate to face the player
    void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position; // Direction to player
        directionToPlayer.y = 0; // Keep the rotation in the horizontal plane

        // If the player is within a significant distance
        if (directionToPlayer.magnitude > 0.1f) 
        {
            // Calculate the desired rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Since the quad's front is facing the opposite direction, invert the rotation
            Quaternion correctedRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + 180, targetRotation.eulerAngles.z);

            // Smoothly rotate toward the player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, correctedRotation, Time.deltaTime * 360); 
        }
    }

    // Get a random point within a specified radius from a given position
    Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    void StartChasingPlayer()
    {
        chasing = true; // Set chasing state
        inSearchRadius = false; // Reset search radius state
        ai.speed = chaseSpeed; // Set speed to chase speed
        audioSource.Play(); // Start playing chase audio
    }

    void PlayerCaught()
    {
        ai.isStopped = true; // Stop the AI movement
        jumpScareImage.gameObject.SetActive(true); // Show the jumpscare image
        StartCoroutine(HandleJumpscare()); // Start the jumpscare coroutine
        player.GetComponent<PlayerController>().LockInput(true); // Lock player input
    }

    IEnumerator HandleJumpscare()
    {
        yield return new WaitForSeconds(jumpScareDuration); // Wait for the duration of the jumpscare
        jumpScareImage.gameObject.SetActive(false); // Hide the jumpscare image
        gameObject.SetActive(false); // Deactivate the AI after the jumpscare
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public AudioLoudnessDetection detector; // Microphone component
    public float loudnessSensitivity = 100; // How sensitive the microphone is
    public float threshold = 0.1f; // The threshold for volume
    public NavMeshAgent ai; // Controls the enemy's movement
    public List<Transform> destinations; // Points for the enemy to move to
    public float walkSpeed; // Enemy's walking speed
    public float chaseSpeed; // Enemy's chasing speed
    public float sightDistance; // Distance at which the enemy can see the player
    public float catchDistance; // Distance at which the enemy catches the player
    public Transform player; // Reference to the player
    public float idleTime, minIdleTime, maxIdleTime; // Idle timing variables
    public bool walking; // Checks if the enemy is walking
    Transform currentDest; // Current destination for the enemy
    int randNum; // Random number for choosing destinations
    Coroutine idleCoroutine; // Manages the idle coroutine
    bool chasing; // Checks if the enemy is chasing the player
    float lostPlayerTime; // Timer for how long the player is out of sight
    public float timeToForgetPlayer; // Time before stopping the chase

    public Image jumpScareImage; // Reference to the image to show when the player is caught

    // Audio components
    public AudioSource chaseAudioSource; // Reference to the AudioSource for chase audio

    void Start()
    {
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        ai.speed = walkSpeed; // Set the initial walking speed
        lostPlayerTime = 0f; // Initialize the lost player timer
        // Hide the jump scare image at the start
        jumpScareImage.gameObject.SetActive(false);
        
        // Set up the audio source
        if (chaseAudioSource != null)
        {
            chaseAudioSource.loop = true; // Enable looping for continuous play
        }
    }

    void Update()
    {
        FacePlayer();

        Vector3 directionToPlayer = player.position - transform.position;

        // Check if the player is within sight distance
        if (directionToPlayer.magnitude < sightDistance)
        {
            StartChasingPlayer();
            lostPlayerTime = 0f; // Reset the timer when player is seen
        }
        else if (chasing)
        {
            lostPlayerTime += Time.deltaTime;

            if (lostPlayerTime > timeToForgetPlayer)
            {
                StopChasingPlayer();
            }
        }

        // Handle walking behavior
        if (walking && !chasing)
        {
            Vector3 dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;

            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                int randNum2 = Random.Range(0, 2);
                if (randNum2 == 0)
                {
                    randNum = Random.Range(0, destinations.Count);
                    currentDest = destinations[randNum];
                }
                else
                {
                    if (idleCoroutine != null)
                    {
                        StopCoroutine(idleCoroutine);
                    }
                    idleCoroutine = StartCoroutine(stayIdle());
                    walking = false;
                }
            }
        }

        // Handle chasing behavior
        if (chasing)
        {
            ai.destination = player.position; // Set destination to player's position
            ai.speed = chaseSpeed; // Use chase speed

            if (Vector3.Distance(player.position, ai.transform.position) <= catchDistance)
            {
                PlayerCaught(); // Call the method for when the player is caught
            }
        }

        // Handle microphone input
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        if (loudness < threshold)
        {
            loudness = 0;
        }
        else
        {
            StartChasingPlayer();
            lostPlayerTime = 0f; // Reset the timer when player is heard
        }
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;
        Vector3 directionAwayFromPlayer = transform.position - player.position;

        if (directionAwayFromPlayer.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionAwayFromPlayer);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 360);
        }
    }

    void StartChasingPlayer()
    {
        chasing = true; // Set chasing state
        walking = false; // Stop walking
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine); // Stop idle routine if active
        }
        if (chaseAudioSource != null && !chaseAudioSource.isPlaying)
        {
            chaseAudioSource.Play(); // Start playing chase audio
        }
    }

    void StopChasingPlayer()
    {
        chasing = false; // Set chasing to false
        lostPlayerTime = 0f; // Reset the timer
        walking = true; // Resume walking behavior
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        if (chaseAudioSource != null)
        {
            chaseAudioSource.Stop(); // Stop the chase audio
        }
    }

    void PlayerCaught()
    {
        ai.isStopped = true;
        gameObject.SetActive(false);
        player.GetComponent<PlayerController>().LockInput(true);
        jumpScareImage.gameObject.SetActive(true);
    }

    IEnumerator stayIdle()
    {
        float idleDuration = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleDuration);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
}

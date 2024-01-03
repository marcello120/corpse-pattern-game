using UnityEngine;

public class WindBlock : MonoBehaviour
{
    public float standardForceMagnitude = 60f; // Set the standard force magnitude
    public float maxForceMagnitude = 250f; // Set the maximum force magnitude

    public Vector2 animationOffset = new Vector2(3.2f, 0f); // Offset for animation position adjustment

    public AudioClip windSoundClip; // The audio clip for wind sound

    private bool playerEntered = false;
    private Transform playerTransform;
    private bool applyingMaxForce = false; // Indicates whether max force is being applied

    private Transform objectTransform; // Declare the objectTransform variable
    private Transform parentTransform; // Reference to the WindWall's transform
    private Transform windAnimatorTransform; // Reference to the Wind_Animator's transform

    [SerializeField] private Animator windAnimator;
    private AudioSource audioSource; // Reference to AudioSource component

    private void Start()
    {
        objectTransform = transform; // Initialize objectTransform in the Start method

        parentTransform = GameObject.Find("WindWall").transform; // Find WindWall object
        windAnimatorTransform = parentTransform.Find("Wind_Animator");
        if (windAnimatorTransform != null)
        {
            windAnimator = windAnimatorTransform.GetComponent<Animator>();
        }

        // Add AudioSource component and configure settings
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = windSoundClip;
    }

    private void Update()
    {
        if (playerEntered)
        {
            if (objectTransform.position.x >= playerTransform.position.x)
            {
                if (!applyingMaxForce)
                {
                    Vector2 localOffset = parentTransform.InverseTransformPoint(playerTransform.position + (Vector3)animationOffset);
                    windAnimatorTransform.localPosition = localOffset;
                    ApplyForce(maxForceMagnitude);
                    RunWindAnimation(true); // Run wind animation
                    applyingMaxForce = true;

                    // Play the sound effect
                    if (windSoundClip != null && !audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
            else if (applyingMaxForce && objectTransform.position.x + (objectTransform.localScale.x * 0.4f) < playerTransform.position.x)
            {
                ApplyForce(standardForceMagnitude * 1.5f);
                applyingMaxForce = false;
                RunWindAnimation(false); // Stop wind animation

                // Stop the sound effect
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    private void ApplyForce(float force)
    {
        // Set the forceMagnitude of the AreaEffector2D component
        AreaEffector2D areaEffector = GetComponent<AreaEffector2D>();
        if (areaEffector != null)
        {
            areaEffector.forceMagnitude = force;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            playerEntered = true;
            playerTransform = other.transform;
            applyingMaxForce = false; // Reset applyingMaxForce flag when player enters
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            playerEntered = false;
            ApplyForce(standardForceMagnitude); // Set the force back to standard when the player exits
            applyingMaxForce = false; // Reset applyingMaxForce flag
            RunWindAnimation(false); // Stop wind animation
        }
    }

    private void RunWindAnimation(bool run)
    {
        if (windAnimator != null)
        {
            windAnimator.SetBool("ShuBool", run); // Set the bool parameter to control animation
        }
    }
}

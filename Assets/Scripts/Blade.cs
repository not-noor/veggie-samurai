using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;
    private Camera mainCamera;
    private Collider sliceCollider;
    private TrailRenderer sliceTrail;

    public AudioClip[] swipeSounds;
    private AudioSource audioSource;

    public Vector3 direction { get; private set; }
    public bool slicing { get; private set; }

    private bool hasPlayedSwipeSound = false; // Flag to track if sound has been played

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlice();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlice();
        }
        else if (slicing)
        {
            ContinueSlice();
        }
    }

    private void StartSlice()
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;

        slicing = true;
        hasPlayedSwipeSound = false;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    private void StopSlice()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void ContinueSlice()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        direction = newPosition - transform.position;

        float velocity = direction.magnitude / Time.deltaTime;

        // Check if velocity exceeds the threshold and play sound once
        if (velocity > minSliceVelocity && !hasPlayedSwipeSound)
        {
            PlaySwipeSound();
            hasPlayedSwipeSound = true; // Ensure sound plays only once per swipe
        }

        sliceCollider.enabled = velocity > minSliceVelocity;
        transform.position = newPosition;
    }

    private void PlaySwipeSound()
    {
        if (audioSource != null && swipeSounds.Length > 0)
        {
            // Randomly pick one of the 7 sounds
            int randomIndex = Random.Range(0, swipeSounds.Length);
            AudioClip randomSound = swipeSounds[randomIndex];

            audioSource.PlayOneShot(randomSound);
        }
    }
}
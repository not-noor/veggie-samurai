using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Collider bombCollider;
    private FlashManager flashManager;
    private Spawner spawner;
    private GameManager gameManager;

    private void Awake()
    {
        bombCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        // Dynamically find FlashManager and GameManager
        flashManager = FindObjectOfType<FlashManager>();
        spawner = FindObjectOfType<Spawner>();
        gameManager = FindObjectOfType<GameManager>();

        if (flashManager == null)
        {
            Debug.LogError("FlashManager not found in the scene!");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }

        if (spawner == null)
        {
            Debug.LogError("Spawner not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        // Trigger the flash effect
        if (flashManager != null)
        {
            flashManager.TriggerFlash();
        }

        // Handle bomb explosion in GameManager
        if (gameManager != null)
        {
            gameManager.HandleBombExplosion();
        }

        // Pause the spawner if available
        if (spawner != null)
        {
            spawner.PauseSpawning(4f);
        }

        // Disable collider and destroy the bomb
        bombCollider.enabled = false;
        Destroy(gameObject);
    }
}

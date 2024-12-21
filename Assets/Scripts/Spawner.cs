using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Spawner : MonoBehaviour
{
    private Collider spawnArea;
    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    [Range(0f, 1f)] public float bombChance = 0.05f;
    public float minAngularVelocity = -0.000001f;
    public float maxAngularVelocity = 0.000001f;
    public AudioClip spawnSound;
    private AudioSource audioSource;
    private bool isPaused = false;

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        while (true) // Infinite loop to spawn objects continuously
        {
            if (isPaused)
            {
                yield return null;
                continue;
            }

            // Randomly select a prefab (fruit or bomb)
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            if (Random.value < bombChance)
            {
                prefab = bombPrefab;
            }

            Vector3 position = new Vector3
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            };

            // Apply a random rotation
            Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(-15f, 15f));

            GameObject fruit = Instantiate(prefab, position, rotation);

            Rigidbody rb = fruit.GetComponent<Rigidbody>();
            
            // Apply upward force to simulate launching
            float force = Random.Range(17f, 22f);
            rb.AddForce(fruit.transform.up * force, ForceMode.Impulse);

            // Apply random torque based on object type
            if (prefab == bombPrefab)
            {
                rb.AddTorque(Random.insideUnitSphere * Random.Range(0.025f, 0.05f), ForceMode.Impulse);
            }
            else
            {
                rb.AddTorque(Random.insideUnitSphere * Random.Range(0.5f, 2.0f), ForceMode.Impulse);
            }

            audioSource.PlayOneShot(spawnSound);

            Destroy(fruit, 5f);

            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
    }

    public void PauseSpawning(float duration)
    {
        StartCoroutine(PauseSpawningRoutine(duration));
    }

    private IEnumerator PauseSpawningRoutine(float duration)
    {
        isPaused = true;
        yield return new WaitForSeconds(duration);
        isPaused = false;
    }
}
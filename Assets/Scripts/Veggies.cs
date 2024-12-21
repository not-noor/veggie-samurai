using UnityEngine;
using System.Collections; 

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
    }
    private void Slice(Vector3 direction, Vector3 position, float force)
    {   
        GameManager.Instance.IncreaseScore(1);    
        Destroy(whole);

        // Instantiate the sliced pieces
        GameObject pieces = Instantiate(sliced, transform.position, transform.rotation);

        // Apply forces to the slices
        Rigidbody[] slices = pieces.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody slice in slices)
        {
            slice.linearVelocity = fruitRigidbody.linearVelocity; // Match original velocity
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
            slice.AddExplosionForce(5f, position, 5f, 2f, ForceMode.Impulse);
        }
        Destroy(pieces, 5f);
    }
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();

            Slice(blade.direction, blade.transform.position, blade.sliceForce);

            

            Destroy(gameObject);
            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                Destroy(collider);
            }
        }
    }
}
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public DynamicSensor sensor;      // Reference to the DynamicSensor component
    public float bulletSpeed = 10f;   // Speed of the bullet
    public int damage = 10;           // Amount of damage the bullet inflicts
    public Vector3 maxBounds;   // Maximum bounds (x, y, z)
    public Vector3 minBounds;          // Maximum bounds (x, y, z)
    [SerializeField]
    private Collider[] detectedEnemies;

    private void Update()
    {
        MoveBullet();
        CheckForCollision();
        CheckOutsideBoundary();
    }

    private void CheckOutsideBoundary()
    {
        // Check if the bullet is outside the bounds
        // Check if the bullet is outside the bounds
        if (transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
            transform.position.z < minBounds.z || transform.position.z > maxBounds.z)
        {
            Destroy(gameObject); // Destroy the bullet if it's outside the bounds
        }
    }

    // Move the bullet forward
    private void MoveBullet()
    {
        if(detectedEnemies.Length < 1)
        {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
    }

    // Use the sensor to check for enemy collision
    private void CheckForCollision()
    {
        detectedEnemies = sensor.Detect();

        if (detectedEnemies.Length > 0)
        {
            Health enemyHealth = detectedEnemies[0].transform.parent.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);  // Apply damage to the enemy
                Destroy(gameObject);  // Destroy the bullet after hitting an enemy
            }
        }
    }
}

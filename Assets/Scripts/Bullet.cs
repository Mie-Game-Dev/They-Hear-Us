using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    private Vector3 target;

    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
        Destroy(gameObject, lifeTime);  // Destroy bullet after its lifetime expires
    }

    private void Update()
    {
        // Move the bullet towards the target
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Optional: destroy the bullet if it gets close enough to the target
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
